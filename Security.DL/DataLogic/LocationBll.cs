using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.Model;
using Security.DataAccess;
using EML.Util;

namespace Security.DataLogic
{
    public class LocationBll
    {
        private LocationDal dal;
        public LocationBll()
        {
            this.dal = new LocationDal();
        }
        #region Location ControlKey

        public List<Location> GetLocationProvideLocationID()
        {
            return this.dal.GetLocationProvideLocationID();
        }

        public List<Location> GetLocationProvideLocationIDByTypeControlKey(string key)
        {
            return this.dal.GetLocationProvideLocationIDByTypeControlKey(key);
        }

        public List<LocationType> GetLocationTypeByTypeControlKey(string key)
        {
            return this.dal.GetLocationTypeByTypeControlKey(key);
        }

        public List<Location> GetLocationByTypeControlKey(string key)
        {
            return this.dal.GetLocationByTypeControlKey(key);
        }

        public List<Location> GetLocationByTypeControlKeyParentID(string key, string parentID)
        {
            return this.dal.GetLocationByTypeControlKeyParentID(key, parentID);
        }

        #endregion

        public List<Location> GetLoctionByCompanyCode(string companyCode)
        {
            return this.dal.GetLoctionByCompanyCode(companyCode);
        }


        public List<Location> GetLocationTreeList()
        {
            return this.dal.GetLocationTreeList();
        }

        public List<LocationType> GetLocationTypeList()
        {
            return this.dal.GetLocationTypeList();
        }

        public bool InsertLocation(Location model)
        {
            model.ID = Guid.NewGuid().ToString();
            return this.dal.Insert(model);
        }


        /// <summary>
        /// 新增库位
        /// </summary>
        public bool InsertLocationCapacity(string companyCode, string LocationID, decimal capacity, string UM)
        {
            return this.dal.InsertLocationCapacity(companyCode, LocationID, capacity, UM);
        }


        /// <summary>
        /// 检查库位是否有库存
        /// </summary>
        /// <param name="locationCode">库位ID</param>
        /// <param name="locationTypeID">库位类型</param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public bool CheckrContainerRelationForLocator(string locationCode, int locationTypeID, string companyCode)
        {
            return this.dal.CheckrContainerRelationForLocator(locationCode, locationTypeID, companyCode);
        }

        /// <summary>
        /// 修改库位
        /// </summary>
        /// <param name="locationID">库位ID</param>
        /// <param name="name">新库位名称</param>
        /// <param name="capacity">新库容</param>
        /// <param name="UM">新库容单位</param>
        /// <returns></returns>
        public bool UpdateLocatorAndCapacity(string locationID, string name, decimal capacity, string UM)
        {
            return this.dal.UpdateLocatorAndCapacity(locationID, name, capacity, UM);
        }

        /// <summary>
        /// 删除库位
        /// </summary>
        /// <param name="locationID">库位ID</param>
        /// <returns></returns>
        public bool DeleteLocatorAndCapacity(string locationID)
        {
            return this.dal.DeleteLocatorAndCapacity(locationID);
        }


        /// <summary>
        /// 检查地点是否已存在
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <param name="locationTypeID">节点类型</param>
        /// <param name="code">code</param>
        /// <returns></returns>
        public bool CheckLocationIsExist(string parentId, int locationTypeID, string code)
        {
            bool isExist = this.dal.CheckLocationIsExist(parentId, code);
            if (!isExist)
            {
                this.locationAll = this.GetLocationTreeList();
                List<Location> tempLocationList = new List<Location>();
                this.getLocationByChild(parentId, tempLocationList);
                this.getLocationByParent(parentId, tempLocationList);
                isExist = tempLocationList.Exists(p => p.Code.ToLower().Equals(code.ToLower()) && p.LocationTypeID.Equals(locationTypeID));
                return isExist;
            }
            return true;
        }

        private List<Location> locationAll = null;

        /// <summary>
        /// 获取地点路径，从末梢到树根
        /// </summary>
        private void getLocationByChild(string parentID, List<Location> tempLocationList)
        {
            Location model = locationAll.FirstOrDefault(p => p.ID.Equals(parentID));
            if (model != null)
            {
                tempLocationList.Insert(0, model);
                if (!string.IsNullOrEmpty(model.ParentID))
                {
                    getLocationByChild(model.ParentID, tempLocationList);
                }
            }
        }

        /// <summary>
        /// 获取地点路径，从树根到末梢
        /// </summary>
        private void getLocationByParent(string id, List<Location> childs)
        {
            List<Location> subList = locationAll.Where(p => p.ParentID.Equals(id)).ToList();
            foreach (Location model in subList)
            {
                childs.Add(model);
                getLocationByParent(model.ID, childs);
            }
        }

        public Location GetLocationByCode(string companyCode, string locationCode, int locationTypeID)
        {
            return this.dal.GetLocationByCode(companyCode, locationCode, locationTypeID);
        }

        public Location GetLocationByName(string companyCode, string locationName, int locationTypeID)
        {
            return this.dal.GetLocationByName(companyCode, locationName, locationTypeID);
        }

        /// <summary>
        /// 删除地点
        /// </summary>
        /// <param name="ids">使用,分隔符</param>
        public bool DeleteLocation(string id, string locationCode)
        {
            return this.dal.DeleteLocation(id, locationCode);
        }

        /// <summary>
        /// 修改地点
        /// </summary>
        public bool UpdateLocation(Location model)
        {
            return this.dal.UpdateLocation(model);
        }
    }
}
