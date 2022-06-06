using System.Collections.Generic;

namespace ProjectLibrary.Config
{
    public class SystemOrderType
    {
        public const int Received = 0;
        public const int Shipping = 1;
        public const int Completed = 2;
        public const int Cancel = 3;
        public const int Exchange = 4;
                     

        public static Dictionary<int, string> OrderType = new Dictionary<int, string>()
                                                                 {
                                                                     {Received, "Đã đăng ký"},
                                                                     {Shipping, "Đang chuyển hàng"},
                                                                     {Completed, "Đã hoàn thành"},
                                                                     {Cancel, "Hủy đơn hàng"},                                                                                                                                        
                                                                     {Exchange, "Khách đổi hàng"},                                                                                                                                        
                                                                   
                                                                 };
    }
}

