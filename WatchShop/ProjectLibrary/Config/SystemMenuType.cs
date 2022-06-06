using System.Collections.Generic;

namespace ProjectLibrary.Config
{
    public class SystemMenuType
    {
        public const int Home = 1;
        public const int Watch = 2;
        public const int Sale = 4;        
        public const int News = 5;
        public const int Location = 6;     
        public const int AboutUs = 7;     
        public const int Contact = 8;              
        public const int Shipping = 9;              

        public static Dictionary<int, string> CategoryType = new Dictionary<int, string>()
                                                                 {
                                                                     {Home, "Trang chủ"},
                                                                     {AboutUs, "Trang giới thiệu"},
                                                                     {News, "Trang bài viết"},                                                                                                                                        
                                                                     {Watch, "Trang sản phẩm"},                                                                    
                                                                     {Location, "Trang vị trí"},
                                                                     {Sale, "Trang Sale"},
                                                                     {Contact, "Trang liên hệ"},
                                                                     {Shipping, "Trang vận chuyển"},
                                                                   
                                                                 };
    }
}

