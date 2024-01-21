using FakeItEasy;
using TaskManagmentAPI.Controllers;
using TaskManagmentAPI.Model;

namespace Task.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int count = 5;
            var db = A.Fake<TaskModel>();
            //A.CallTo(() => {}).Return(Task.FromResult());
            var controller = new TaskModelsController(db);
        }
    }
}