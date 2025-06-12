using System.Windows.Controls;
using CRMSystemApp.Models;

namespace CRMSystemApp.Views
{
    public partial class EmailDetailsCardView : UserControl
    {
        private readonly Message _email;

        public EmailDetailsCardView(Message email)
        {
            InitializeComponent();

            _email = email;

            SubjectTextBlock.Text = _email.Subject;
            BodyTextBlock.Text = _email.Body;
            ReceiverNameTextBlock.Text = _email.ReceiverName;
            EmailTextBlock.Text = _email.Email;
            SentAtTextBlock.Text = _email.SentAt.ToString("dd.MM.yyyy");
        }
    }
}