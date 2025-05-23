﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SACS.Common;
using SACS.Web.Controllers;

namespace SACS.Web.Areas.Administration.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
[Area("Administration")]
public class AdministrationController : BaseController
{
}