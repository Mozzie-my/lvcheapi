namespace XlcToolBox.Model
{
    public class TbkTransViewModel
    {
        public int code { get; set; }
        public Datum[] data { get; set; }
        public string msg { get; set; }
    }

    public class Datum
    {
        public int code { get; set; }
        public Data data { get; set; }
        public string link { get; set; }
        public string match { get; set; }
        public object msg { get; set; }
        public string seller_id { get; set; }
        public int sort { get; set; }
    }

    public class Data
    {
        public string biz_scene_id { get; set; }
        public string bybtqdyh { get; set; }
        public string commission_rate { get; set; }
        public string content { get; set; }
        public string coupon_amount { get; set; }
        public string coupon_end_time { get; set; }
        public string coupon_id { get; set; }
        public string coupon_info { get; set; }
        public string coupon_link { get; set; }
        public string coupon_src_scene { get; set; }
        public string coupon_start_fee { get; set; }
        public string d_title { get; set; }
        public string gid { get; set; }
        public string isTmall { get; set; }
        public string is_multiple { get; set; }
        public string item_click_url { get; set; }
        public string item_id { get; set; }
        public string item_link { get; set; }
        public string kz_address { get; set; }
        public string long_link { get; set; }
        public string main_pic { get; set; }
        public string new_data { get; set; }
        public string new_tpwd { get; set; }
        public string originUrl { get; set; }
        public string origin_id { get; set; }
        public string origin_tpwd { get; set; }
        public string original_price { get; set; }
        public string pic { get; set; }
        public string price { get; set; }
        public string rate { get; set; }
        public string sales { get; set; }
        public string seller_id { get; set; }
        public string shop_logo { get; set; }
        public string shop_name { get; set; }
        public string short_link { get; set; }
        public string source { get; set; }
        public string status_code { get; set; }
        public string title { get; set; }
        public string tlj_black { get; set; }
        public string type { get; set; }
    }


}
