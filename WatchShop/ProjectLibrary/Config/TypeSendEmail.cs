using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ProjectLibrary.Config
{
    public class TypeSendEmail
    {
        public static int Contact = 1;
        public static int Order = 2;

        public static List<ListItem> ListTypeSendEmail()
        {
            var listTypeSendEmail = new List<ListItem>
            {
                new ListItem
                {
                    Value = Contact.ToString(),
                    Text = "Liên hệ",
                },
                 new ListItem
                {
                    Value = Order.ToString(),
                    Text = "Đặt hàng",
                },
            };

            return listTypeSendEmail;
        }
    }
}