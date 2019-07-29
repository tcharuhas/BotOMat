using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API {
    [Produces ("application/json")]
    [Route("api/robots")]
    public class RobotController : Controller 
    {
        public static RobotDataBase Database
            = new RobotDataBase {
                Robots = new List<Robot> {
                   
                }
            };

        //  Private methods that are called in API endpoints - separated to clean up code & avoid 
        //  future duplication of code  

        private Robot Find(string robotName){
            // Linq: Select, Where, Aggregate, GroupBy
             return Database.Robots.Where(list => list.Name == robotName).FirstOrDefault();
        }

        private Task taskUpdate(Robot robotToUpdate,String task) {
            Task foundTask = robotToUpdate.Tasks.Where(item => item.Name == task).FirstOrDefault();
            return foundTask;
        }

        [HttpGet]
        [Route ("")]
        public ActionResult<RobotDataBase> Get() 
        {
            return Ok(Database);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<Robot> Create([FromBody] Robot robot) {
                Database.Robots.Add(robot);
            return Ok(robot);
        }

        [HttpDelete]
        [Route("delete/{robotName}")]
        public ActionResult<Robot> Delete([FromRoute] String robotName) {
            Robot toDelete = Find(robotName);
            Database.Robots.Remove(toDelete);
            return Ok(toDelete);
        }

        [HttpPost]
        [Route("{robotName}/{task}")]
        public ActionResult<Task> Start([FromRoute] String robotName, [FromRoute] String task) {
            var robotToUpdate = Find(robotName);
            var taskToUpdate = taskUpdate(robotToUpdate,task);
            taskToUpdate.State = TaskState.Completed;
            robotToUpdate.completedTasks++;
            return Ok(taskToUpdate);
        }

    }
}
