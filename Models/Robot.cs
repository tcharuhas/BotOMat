using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API.Models
{
    public class RobotDataBase {
        //  I utilized a list as my in memory 'database' for persistent data
        public List<Robot> Robots
            = new List<Robot>();
    }

    public class Robot
    {
        public string Name = "";
        public string Type = "";
        public int completedTasks = 0;
        public List<Task> Tasks
            = new List<Task>();
    }

    public class Task
    {
        public string Name = "";
        public long time = 0;
        public TaskState State = TaskState.Unstarted;
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskState
    {
        Unstarted,
        InProgress,
        Completed
    }

}