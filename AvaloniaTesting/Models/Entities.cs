using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AvaloniaTestingProgramm
{
    class Entities
    {   //Обычные вопросы
        public class BaseQuestions
        {
            public int QuestionID { get; set; }
            public string Qouestion { get; set; }
            public string CorrectAnser { get; set; }
            public string IncorrectAnswer1 { get; set; }
            public string IncorrectAnswer2 { get; set; }
            public string IncorrectAnswer3 { get; set; }

        }
        //Результаты тестов
        public class Results
        {
            public int ResultID { get; set; }
            public string TestName { get; set; }
            public int Correct { get; set; }
            public int Incorrect { get; set; }
        }


        public class TestsDatabase
        {
            public List<BaseQuestions> BaseQuestions { get; set; } = new List<BaseQuestions>();
        }

        public class ResultsDatabase
        {
            public List<Results> Results { get; set; } = new List<Results>();
        }
    }
}
