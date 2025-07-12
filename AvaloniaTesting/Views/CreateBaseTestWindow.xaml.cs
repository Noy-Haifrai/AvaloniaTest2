using System.IO;
using System.Windows;
using System.Windows.Controls;
using static AvaloniaTestingProgramm.Entities;
using static AvaloniaTestingProgramm.Controller.Checks;

namespace AvaloniaTestingProgramm
{
    public partial class CreateBaseTestWindow : Window
    {
        private bool isTestNameLocked = false;
        private TestsDatabase currentTest = new TestsDatabase();

        public CreateBaseTestWindow()
        {
            InitializeComponent();
            if (!Directory.Exists("Tests"))
                Directory.CreateDirectory("Tests");
        }

        private void ClearFields()
        {
            Question.Text = "";
            Correct.Text = "";
            incorrect1.Text = "";
            incorrect2.Text = "";
            incorrect3.Text = "";
            Question.Focus();
        }

        private void createNextQuestioBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateQuestionInputs(Correct.Text, incorrect1.Text, TestName.Text, isTestNameLocked)) return;

            if (!isTestNameLocked)
            {
                isTestNameLocked = true;
                TestName.IsEnabled = false;
            }

            currentTest.BaseQuestions.Add(new BaseQuestions
            {
                QuestionID = currentTest.BaseQuestions.Count + 1,
                Qouestion = Question.Text.Trim(),
                CorrectAnser = Correct.Text.Trim(),
                IncorrectAnswer1 = incorrect1.Text.Trim(),
                IncorrectAnswer2 = string.IsNullOrWhiteSpace(incorrect2.Text) ? null : incorrect2.Text.Trim(),
                IncorrectAnswer3 = string.IsNullOrWhiteSpace(incorrect3.Text) ? null : incorrect3.Text.Trim()
            });

            ClearFields();
        }

        private void saveAndExitTestBtn_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на частично заполненный вопрос
            bool hasPartialQuestion = !string.IsNullOrWhiteSpace(Question.Text) ||
                                    !string.IsNullOrWhiteSpace(Correct.Text) ||
                                    !string.IsNullOrWhiteSpace(incorrect1.Text);

            if (hasPartialQuestion)
            {
                if (!ValidateInputs(Correct.Text, incorrect1.Text)) return;

                currentTest.BaseQuestions.Add(new BaseQuestions
                {
                    QuestionID = currentTest.BaseQuestions.Count + 1,
                    Qouestion = Question.Text.Trim(),
                    CorrectAnser = Correct.Text.Trim(),
                    IncorrectAnswer1 = incorrect1.Text.Trim(),
                    IncorrectAnswer2 = string.IsNullOrWhiteSpace(incorrect2.Text) ? null : incorrect2.Text.Trim(),
                    IncorrectAnswer3 = string.IsNullOrWhiteSpace(incorrect3.Text) ? null : incorrect3.Text.Trim()
                });
            }

            if (!ValidateTestContent(currentTest.BaseQuestions.Count)) return;

            string fileName = Path.Combine("Tests", $"{TestName.Text}.json");
            JsonDataService.SaveData(currentTest, fileName);
            Close();
        }
    }
}