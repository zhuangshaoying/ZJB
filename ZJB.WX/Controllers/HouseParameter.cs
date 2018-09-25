using System;
namespace ZJB.WX.Models
{
    public class HouseParameter : ICloneable
    {
        private string trade = "esf";
        private string city = "xm";
        private int? distrct = null;
        private int? region = null;
        private int? userType = null;
        private int? layout = null;
        private int? buildType = null;
        private int? minPrice = null;
        private int? maxPrice = null;
        private int? minArea = null;
        private int? maxArea = null;
        private int? postTimeDay = null;      
        private int? orderBy = null;
        private int? pageSize = null;
        private int? page= null;
        private string keyWord = null;
        
        public int? Page
        {
            get { return page; }
            set { page = value; }
        }

        public string Trade
        {
            get { return trade; }
            set { trade = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public int? Region
        {
            get { return region; }
            set { region = value; }
        }

        public int? Distrct
        {
            get { return distrct; }
            set { distrct = value; }
        }

        public int? UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public int? Layout
        {
            get { return layout; }
            set { layout = value; }
        }

     

        public int? BuildType
        {
            get { return buildType; }
            set { buildType = value; }
        }

    
        public int? MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }

        public int? MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        public int? MinArea
        {
            get { return minArea; }
            set { minArea = value; }
        }

        public int? MaxArea
        {
            get { return maxArea; }
            set { maxArea = value; }
        }

        public int? PostTimeDay
        {
            get { return postTimeDay; }
            set { postTimeDay = value; }
        }

        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }

        public int? OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int? PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return trade
                + "_" + region
                + "_" + distrct
                + "_" + userType
                + "_" + layout
                + "_" + buildType
                + "_" + minPrice
                + "_" + maxPrice
                + "_" + minArea
                + "_" + maxArea
                + "_" + keyWord
                + "_" + orderBy
                + "_" + pageSize
                + "_" + page;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(HouseParameter)) return false;
            return Equals((HouseParameter)obj);
        }

        public bool Equals(HouseParameter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.trade == trade
                && Equals(other.region, region)
                && Equals(other.distrct, distrct)
                && other.userType == userType
                && other.layout == layout
                && other.buildType == buildType
                && other.minPrice == minPrice
                && other.maxPrice == maxPrice
                && other.minArea == minArea
                && other.maxArea == maxArea
                && Equals(other.keyWord, keyWord)
                && other.orderBy == orderBy;
        }

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        int result = (trade != null ? trade.GetHashCode() : 0);
        //        result = (result * 397) ^ (region != null ? region.GetHashCode() : 0);
        //        result = (result * 397) ^ (address != null ? address.GetHashCode() : 0);
        //        result = (result * 397) ^ userType;
        //        result = (result * 397) ^ layout;
        //        result = (result * 397) ^ buildType;
        //        result = (result * 397) ^ minPrice;
        //        result = (result * 397) ^ maxPrice;
        //        result = (result * 397) ^ minArea;
        //        result = (result * 397) ^ maxArea;
        //        result = (result * 397) ^ (keyWord != null ? keyWord.GetHashCode() : 0);
        //        result = (result * 397) ^ orderBy;
        //        result = (result * 397) ^ pageSize;
        //        result = (result * 397) ^ page;
        //        return result;
        //    }
        //}
    }
}
