using TMonitBackend.Models;

namespace TMonitBackend.Services
{
    public class ValidationQuery
    {
        public ValidationQuery(){}
        private static Dictionary<object, Task<bool>> _validationTasks = new Dictionary<object, Task<bool>>();
        private static Dictionary<object, bool> _validationRegistries = new Dictionary<object, bool>(); 
        
        public string add(Task<bool> whatnext)
        {
            var guid = new Guid();
            _validationTasks.Add(guid, whatnext);
            return guid.ToString();
        }

        public bool validate(string guid)
        {
            if (_validationTasks.ContainsKey(guid))
            {
                var task = _validationTasks[guid];
                task.Start();
                _validationTasks.Remove(guid);
                return task.Result;
            }
            return false;
        }

        public string add(object identity, Task<bool> whatnext)
        {
            var guid = new Guid();
            _validationTasks.Add(new Tuple<object, object>(identity, guid), whatnext);
            return guid.ToString();
        }

        public bool validate(object identity, string guid)
        {
            var key = new Tuple<object, object>(identity, guid);
            if (_validationTasks.ContainsKey(key))
            {
                var task = _validationTasks[key];
                task.Start();
                _validationTasks.Remove(key);
                return task.Result;
            }
            return false;
        }
    }
}