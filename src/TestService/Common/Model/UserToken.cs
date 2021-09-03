using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
    public class UserToken
    {
        public string UserNumber { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 数据权限类别(1:区域专属 2:行业专属 6:区域 且 产品线 7:区域 或 产品线 8:区域 且 行业 9:区域 或 行业)
        /// </summary>
        public string AreaTrade { get; set; }

        /// <summary>
        /// 是否启用新的数据权限
        /// </summary>
        public string DYNewDataRight { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public string IsSuper { get; set; }

        /// <summary>
        /// 子用户编码
        /// </summary>
        public string SubUser { get; set; }

        /// <summary>
        /// 总部Code
        /// </summary>
        public string DataHeadCode { get; set; }

        /// <summary>
        /// 分部Code
        /// </summary>
        public string DivisionCode { get; set; }

        /// <summary>
        /// 区域Code
        /// </summary>
        public string DataAreaCode { get; set; }

        /// <summary>
        /// 办事处Code
        /// </summary>
        public string DataAgencyCode { get; set; }

        /// <summary>
        /// 小组Code
        /// </summary>
        public string DataGroupCode { get; set; }

        /// <summary>
        /// 用户管辖小行业
        /// </summary>
        public string DataTrade2 { get; set; }

        /// <summary>
        /// 用户管辖大行业
        /// </summary>
        public string DataTrade1 { get; set; }

        /// <summary>
        /// 客户新数据权限
        /// </summary>
        public string NewDataCusRight { get; set; }

        /// <summary>
        /// 项目新数据权限
        /// </summary>
        public string NewDataPrjRight { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 默认页面
        /// </summary>
        public string HomePage { get; set; }

        /// <summary>
        /// 默认角色
        /// </summary>
        public string RoleCode { get; set; }

    }

    public class UserTokenJava
    {
        public string deptName { get; set; }
        public string code { get; set; }
        public string deptId { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
        public string deptCode { get; set; }
        public string username { get; set; }
        public string status { get; set; }
        /// <summary>
        /// 默认页面
        /// </summary>
        public string HomePage { get; set; }

        /// <summary>
        /// 默认角色
        /// </summary>
        public string RoleCode { get; set; }

        public string Role { get; set; }
    }
}
