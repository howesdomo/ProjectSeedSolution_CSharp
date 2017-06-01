using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Security.DataAccess;
using Security.Model;

namespace Security.DataLogic
{
    public class ProvideLocationBll
    {
        private ProvideLocationDal dal;

        public ProvideLocationBll()
        {
            this.dal = new ProvideLocationDal();
        }


        public List<ProvideLocation> GetProvideLocationByParent(string id)
        {
            return this.dal.GetProvideLocationByParent(id);
        }


        ///// <summary>
        ///// 初始化 Location集合. 参数empty,无实际意义
        ///// </summary>>
        //public ProvideLocationBll(string empty)
        //{
        //    this.dal = new ProvideLocationDal();

        //    LocationBll locationBll = new LocationBll();
        //    this.locationAll.AddRange(locationBll.GetLocationTreeList());
        //}

        //private List<Location> locationAll = new List<Location>();

        public void InsertProvideLocation(List<ProvideLocation> locationlist)
        {
            foreach (ProvideLocation model in locationlist)
            {
                this.dal.InsertProvideLocation(model);
            }
        }

        public bool InsertProvideLocation(ProvideLocation newLocation)
        {
            return this.dal.InsertProvideLocation(newLocation);
        }


        public void InsertProvideLocation(List<Location> list)
        {
            foreach (Location item in list)
            {
                this.InsertProvideLocation(item);
            }
        }

        public bool InsertProvideLocation(Location newLocation)
        {
            ProvideLocation provideLocation = this.GetProvideLocation(newLocation);
            return this.dal.InsertProvideLocation(provideLocation);
        }

        public ProvideLocation InsertProvideLocationForNew(Location newLocation)
        {
            ProvideLocation provideLocation = this.GetProvideLocation(newLocation);
            if (this.dal.InsertProvideLocation(provideLocation))
            {
                return provideLocation;
            }
            return null;
        }

        /// <summary>
        /// 获取所有供给地点
        /// </summary>
        public List<ProvideLocation> GetProvideLocationAll()
        {
            return this.dal.GetProvideLocationList();
        }

        /// <summary>
        /// 获取所有供给地点
        /// </summary>
        public List<ProvideLocation> GetProvideLocationByCompanyCode(string companyCode)
        {
            var lst = this.dal.GetProvideLocationList();
            return lst.Where(p => p.LocationCode1.ToUpper() == companyCode.ToUpper()).ToList();
        }

        /// <summary>
        /// 获取所有供给地点
        /// </summary>
        public List<ProvideLocation> GetProvideLocationByCompanyCode(string companyCode, int type)
        {
            var lst = this.dal.GetProvideLocationList();
            return lst.Where(p => p.LocationCode1.ToUpper() == companyCode.ToUpper() && p.LocationTypeID_5 == type).ToList();
        }

        /// <summary>
        /// 获取地点路径
        /// </summary>
        public ProvideLocation GetProvideLocationByCode(string companyCode, string locationCode, int locationTypeID)
        {
            return this.dal.GetProvideLocationByCode(companyCode, locationCode, locationTypeID);
        }

        ///// <summary>
        ///// 根据公司代号，取得该公司的树结构
        ///// </summary>
        ///// <param name="companyCode">公司代号</param>
        //public List<Location> GetCompanyLocationByCompanyCode(string companyCode)
        //{
        //    List<Location> locationTop = this.locationAll.Where(p => p.ParentID.Equals(string.Empty)).ToList();
        //    Location companyLocation = locationTop.FirstOrDefault(p => p.Code.Equals(companyCode));//公司
        //    if (companyLocation == null)
        //    {
        //        return new List<Location>();
        //    }
        //    List<Location> companyLocationAll = new List<Location>();
        //    this.getLocationByParentID(companyLocation.ID, companyLocationAll);//公司子集

        //    return companyLocationAll;
        //}

        ///// <summary>
        ///// 根据父级ID，取得它所包含的子级
        ///// </summary>
        //private void getLocationByParentID(string id, List<Location> childList)
        //{
        //    List<Location> subList = this.locationAll.Where(p => p.ParentID.Equals(id)).ToList();
        //    foreach (Location model in subList)
        //    {
        //        childList.Add(model);
        //        getLocationByParentID(model.ID, childList);
        //    }
        //}


        /// <summary>
        /// 根据当前的Location,生成ProvideLocation路径
        /// </summary>
        public ProvideLocation GetProvideLocation(Location currentLocation)
        {
            List<Location> locationTree = new List<Location>();
            ProvideLocation model = new ProvideLocation();
            if (string.IsNullOrEmpty(currentLocation.ParentID))
            {
                model.LocationCode1 = currentLocation.Code;
                model.LocationName1 = currentLocation.Name;
                model.LocationTypeID_1 = currentLocation.LocationTypeID;
                model.LocationCode5 = currentLocation.Code;
                model.LocationName5 = currentLocation.Name;
                model.LocationTypeID_5 = currentLocation.LocationTypeID;
            }
            else
            {
                LocationBll bll = new LocationBll();
                List<Location> locationAll = bll.GetLoctionByCompanyCode(currentLocation.CompanyCode);
                this.getLocationByChild(currentLocation.ParentID, locationTree, locationAll);
                locationTree.Add(currentLocation);
                model = this.getProvideLocationByLocationList(locationTree);
            }
            model.ID = Guid.NewGuid().ToString();
            return model;
        }


        /// <summary>
        /// 根据某个子级的父ID,取得它所在的路径
        /// </summary>
        private void getLocationByChild(string parentID, List<Location> locationTree, List<Location> locationAll)
        {
            Location model = locationAll.FirstOrDefault(p => p.ID.Equals(parentID));
            if (model != null)
            {
                locationTree.Insert(0, model);
                if (!string.IsNullOrEmpty(model.ParentID))
                {
                    getLocationByChild(model.ParentID, locationTree, locationAll);
                }
            }
        }

        /// <summary>
        /// 根据某公司树结构，新建车间路径
        /// </summary>
        private ProvideLocation getProvideLocationByLocationList(List<Location> locationTree)
        {
            ProvideLocation model = new ProvideLocation();
            if (locationTree.Count == 1)
            {
                model.LocationCode5 = locationTree[0].Code;
                model.LocationName5 = locationTree[0].Name;
                model.LocationTypeID_5 = locationTree[0].LocationTypeID;
            }
            else
            {
                int count = locationTree.Count - 1;
                for (int i = 0; i < locationTree.Count; i++)
                {
                    Location l = locationTree[i];
                    if (i == count)
                    {
                        model.LocationCode5 = locationTree[i].Code;
                        model.LocationName5 = locationTree[i].Name;
                        model.LocationTypeID_5 = locationTree[i].LocationTypeID;
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                model.LocationCode1 = l.Code;
                                model.LocationName1 = l.Name;
                                model.LocationTypeID_1 = l.LocationTypeID;
                                break;
                            case 1:
                                model.LocationCode2 = l.Code;
                                model.LocationName2 = l.Name;
                                model.LocationTypeID_2 = l.LocationTypeID;
                                break;
                            case 2:
                                model.LocationCode3 = l.Code;
                                model.LocationName3 = l.Name;
                                model.LocationTypeID_3 = l.LocationTypeID;
                                break;
                            default:
                                model.LocationCode4 = l.Code;
                                model.LocationName4 = l.Name;
                                model.LocationTypeID_4 = l.LocationTypeID;
                                break;
                        }
                    }
                }
            }

            return model;
        }

    }
}
