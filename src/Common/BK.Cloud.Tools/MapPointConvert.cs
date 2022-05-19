using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BK.Cloud.Tools
{
    public class MapPointConvert
    {
        const double pi = 3.14159265358979324;

        //
        // Krasovsky 1940
        //
        // a = 6378245.0, 1/f = 298.3
        // b = a * (1 - f)
        // ee = (a^2 - b^2) / a^2;
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;
        private const double EARTH_RADIUS = 6378.137;

        public static void gpstobaidu(double wgLat, double wgLon, out double mgLat, out double mgLon)
        {
            double glat, glon;
            transform(wgLat, wgLon, out glat, out glon);
            bd_encrypt(glat, glon, out mgLat, out mgLon);
        }

        public static void baidutogps(double wgLat, double wgLon, out double mgLat, out double mgLon)
        {
            double glat, glon;
            gpstobaidu(wgLat, wgLon, out glat, out glon);
            mgLat = 2 * wgLat - glat;
            mgLon = 2 * wgLon - glon; 
        }

        /// <summary>
        /// GPS转google坐标进行转换
        /// </summary>
        /// <param name="wgLat"></param>
        /// <param name="wgLon"></param>
        /// <param name="mgLat"></param>
        /// <param name="mgLon"></param>
        public static void transform(double wgLat, double wgLon, out double mgLat, out double mgLon)
        {
            if (outOfChina(wgLat, wgLon))
            {
                mgLat = wgLat;
                mgLon = wgLon;
                return;
            }
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            mgLat = wgLat + dLat;
            mgLon = wgLon + dLon;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }


        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// gps坐标转谷歌坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// gps坐标转谷歌坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        /// <summary>
        /// 谷歌坐标转百度坐标
        /// </summary>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        static void bd_encrypt(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
        {

            double x = gg_lon, y = gg_lat;

            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);

            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);

            bd_lon = z * Math.Cos(theta) + 0.0065;

            bd_lat = z * Math.Sin(theta) + 0.006;

        }

        /// <summary>
        /// 百度坐标转谷歌坐标
        /// </summary>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        static void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
        {

            double x = bd_lon - 0.0065, y = bd_lat - 0.006;

            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);

            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);

            gg_lon = z * Math.Cos(theta);

            gg_lat = z * Math.Sin(theta);

        }

        public class LatLng
        {

            public LatLng()
            {

            }

            public LatLng(double latitude, double longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }

            public double latitude { get; set; }

            public double longitude { get; set; }
        }

        private void Test()
        {
            double lng = 112.837480;
            double  lat= 28.228440;
           var p= transformFromWGSToGCJ(new LatLng(lat, lng));
           Trace.Write(p.latitude + "," + p.longitude);
        }

        public static LatLng transformFromWGSToGCJ(LatLng wgLoc)
        {

                 //如果在国外，则默认不进行转换
                if (outOfChina(wgLoc.latitude, wgLoc.longitude)) {
                         return new LatLng(wgLoc.latitude, wgLoc.longitude);
                 }
                double dLat = transformLat1(wgLoc.longitude - 105.0,
                                 wgLoc.latitude - 35.0);
                 double dLon = transformLon1(wgLoc.longitude - 105.0,
                                 wgLoc.latitude - 35.0);
                 double radLat = wgLoc.latitude / 180.0 * Math.PI;
                 double magic = Math.Sin(radLat);
                 magic = 1 - ee * magic * magic;
                 double sqrtMagic = Math.Sqrt(magic);
                 dLat = (dLat * 180.0)/ ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
                 dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);

                 return new LatLng(wgLoc.latitude + dLat, wgLoc.longitude + dLon);
         }

        public static double transformLat1(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                            + 0.2 * Math.Sqrt(x > 0 ? x : -x);
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x
                            * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0
                            * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y
                            * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double transformLon1(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                            * Math.Sqrt(x > 0 ? x : -x);
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x
                            * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0
                            * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x
                            / 30.0 * Math.PI)) * 2.0 / 3.0;
            return ret;
        }
         
 
        /// <summary>
        ///计算两点GPS坐标的距离
        /// </summary>
        /// <param name="lng1">第一点的纬度坐标</param>
        /// <param name="lat1">第一点的经度坐标</param>
        /// <param name="lng2">第二点的纬度坐标</param>
        /// <param name="lat2">第二点的经度坐标</param>
        /// <returns></returns>
        public static double Distance(double lng1, double lat1, double lng2, double lat2)
        {
            double jl_jd = 102834.74258026089786013677476285;
            double jl_wd = 111712.69150641055729984301412873;
            double b = Math.Abs((lat1 - lat2) * jl_jd);
            double a = Math.Abs((lng1 - lng2) * jl_wd);
            return Math.Sqrt((a * a + b * b));
        }
    }
}
