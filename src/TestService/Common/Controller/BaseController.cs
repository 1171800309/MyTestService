using Common.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Common.Controller
{
    [EnableCors("AllowCors")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        protected UserToken GetCurrentUser()
        {
            var claim = User.Claims;
            if (claim.Count() > 0)
            {
                var usernumber = claim.FirstOrDefault(a => a.Type == "UserNumber")?.Value;//当前用户编码
                var usercode = claim.FirstOrDefault(a => a.Type == "UserCode")?.Value;//当前用户编码
                var username = claim.FirstOrDefault(a => a.Type == "UserName")?.Value;//当前用户
                var deptcode = claim.FirstOrDefault(a => a.Type == "DeptCode")?.Value;//部门编码
                var deptname = claim.FirstOrDefault(a => a.Type == "DeptName")?.Value;//部门

                var areatrade = claim.FirstOrDefault(a => a.Type == "AreaTrade")?.Value;//数据权限类别
                var dynewdataright = claim.FirstOrDefault(a => a.Type == "DYNewDataRight")?.Value;//是否启用新的数据权限
                var issuper = claim.FirstOrDefault(a => a.Type == "IsSuper")?.Value;//是否超级管理员
                var subuser = claim.FirstOrDefault(a => a.Type == "SubUser")?.Value;//子用户编码
                var dataheadcode = claim.FirstOrDefault(a => a.Type == "DataHeadCode")?.Value;//总部code
                var divisioncode = claim.FirstOrDefault(a => a.Type == "DivisionCode")?.Value;//分部code
                var dataareacode = claim.FirstOrDefault(a => a.Type == "DataAreaCode")?.Value;//大区code
                var dataagencycode = claim.FirstOrDefault(a => a.Type == "DataAgencyCode")?.Value;//办事处code
                var datagroupcode = claim.FirstOrDefault(a => a.Type == "DataGroupCode")?.Value;//小组code
                var datatrade2 = claim.FirstOrDefault(a => a.Type == "DataTrade2")?.Value;//小行业
                var datatrade1 = claim.FirstOrDefault(a => a.Type == "DataTrade1")?.Value;//大行业
                var newdatacusright = claim.FirstOrDefault(a => a.Type == "NewDataCusRight")?.Value;//客户新数据权限
                var newdataprjright = claim.FirstOrDefault(a => a.Type == "NewDataPrjRight")?.Value;//项目新数据权限
                var pagecode = claim.FirstOrDefault(a => a.Type == "HomePage")?.Value;//默认页面
                var rolecode = claim.FirstOrDefault(a => a.Type == "RoleCode")?.Value;//默认页面

                return new UserToken
                {
                    UserNumber = usernumber,
                    UserCode = usercode,
                    UserName = username,
                    DeptCode = deptcode,
                    DeptName = deptname,
                    AreaTrade = areatrade,
                    DYNewDataRight = dynewdataright,
                    IsSuper = issuper,
                    SubUser = subuser,
                    DataHeadCode = dataheadcode,
                    DivisionCode = divisioncode,
                    DataAreaCode = dataareacode,
                    DataAgencyCode = dataagencycode,
                    DataGroupCode = datagroupcode,
                    DataTrade2 = datatrade2,
                    DataTrade1 = datatrade1,
                    NewDataCusRight = newdatacusright,
                    NewDataPrjRight = newdataprjright,
                    HomePage = pagecode,
                    RoleCode = rolecode
                };
            }
            else { return null; }
        }
    }
}
