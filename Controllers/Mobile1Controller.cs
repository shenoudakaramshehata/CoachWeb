using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Coach.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class Mobile1Controller : Controller
    {

        private readonly CoachContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;


        public Mobile1Controller(CoachContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment,
            IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _emailSender = emailSender;


        }




        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)
        {

            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null && roles.FirstOrDefault() == "Trainer")
                    {
                        var trainer = await _context.Trainers.FindAsync(user.EntityId);

                        return Ok(new { Status = "Success", Message = "User Login successfully!", user, trainer });
                    }
                }
            }
            var invalidResponse = new { status = false };
            return Ok(invalidResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            var trainer = new Trainer();

            if (registrationModel.Pic != null)
            {
                var bytes = Convert.FromBase64String(registrationModel.Pic);
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Trainer");
                string uniqePictureName = Guid.NewGuid() + ".jpeg";
                string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                trainer.Pic = uniqePictureName;
            }
            trainer.Mobile = registrationModel.Mobile;
            trainer.Tele = registrationModel.Tele;
            trainer.FullNameAr = registrationModel.FullNameAr;
            trainer.FullNameEn = registrationModel.FullNameEn;
            trainer.CountryId = registrationModel.CountryId;
            trainer.Fax = registrationModel.Fax;
            trainer.Email = registrationModel.Email;
            trainer.GenderId = registrationModel.GenderId;
            trainer.SectionId = registrationModel.SectionId;
            trainer.DescriptionEn = registrationModel.DescriptionEn;
            trainer.DescriptionAr = registrationModel.DescriptionAr;

            _context.Trainers.Add(trainer);
            _context.SaveChanges();
            if (!(trainer.TrainerId > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }


            var user = new ApplicationUser
            {
                UserName = registrationModel.Email,
                Email = registrationModel.Email,
                PhoneNumber = registrationModel.Mobile,
                EntityId = trainer.TrainerId,
                EntityName = 2

            };

            var result = await _userManager.CreateAsync(user, registrationModel.Password);

            if (!result.Succeeded)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }
            string html =
@"
<div style='display: table;margin:auto; border: 1px solid #ccc;'>
<tbody style='
    text-align: center;
    margin: auto;
' ><tr>
                      <td>
                        
                        <table style='margin:auto' align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody  style='margin:auto'><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody ><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='40' class='m_-3593417453714472844spacingHeight-40' style='line-height:40px;min-height:40px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='center'>
                                                <a href='http://codewarenet-001-site2.dtempurl.com/'>
<img src='https://g.top4top.io/p_2210qvkhz1.png' id='m_-3593417453714472844logoBlock-4' border='0' alt='' width='246' style='display:block' class='CToWUd'>
</a>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='10' style='line-height:10px;min-height:10px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td id='m_-3593417453714472844bodyText-6' style='font-family:'Inter',sans-serif;font-size:15px;line-height:150%;color:#000'>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>Hey there!</p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>We're <strong>extremely </strong>proud to let you know that we've just released  a <strong>brand new</strong> update to our bestselling beginner's PHP book: <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MDkmZD1rNWIzZjZ5.0gsFAHlQ2wiLQX9hfRptWdDnKidAzGFt3PlWw5uFsnk' style='word-break:break-word;font-family:'Inter',sans-serif;color:#7232fa;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MDkmZD1rNWIzZjZ5.0gsFAHlQ2wiLQX9hfRptWdDnKidAzGFt3PlWw5uFsnk&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0XMpaQk1mgtlBOMVyoVfbh'><strong>PHP &amp; MySQL: Novice to Ninja</strong></a><em>.</em> Now in its seventh edition, we've worked with expert author Tom&nbsp;Butler to thoroughly revise the content to cover PHP 8.1, the latest version of the popular open-source web development scripting language . In addition, we've moved to a Docker-based setup to make installation of PHP and the setup of the plethora of&nbsp;examples shown in the book a
snap.<br></p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'><em><a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MjEmZD10OWkzZjln.64kKb8v-2vPyPd4E1rufVfBAlkdo2Eje-OOa13e2yQU' style='word-break:break-word;font-family:'Inter',sans-serif;color:#7232fa;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MjEmZD10OWkzZjln.64kKb8v-2vPyPd4E1rufVfBAlkdo2Eje-OOa13e2yQU&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0d6cxbPjRYLdNfKJ-FOGnc'>PHP &amp; MySQL: Novice to Ninja</a></em> remains the <strong>easiest</strong> and <strong>best</strong> way to learn PHP. It doesn't assume any prior programming experience.</p>
                                                <ul style='margin-top:0px;margin-bottom:10px'></ul>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                  
                                                  </td>
                                                </tr>
                                              </tbody></table>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td id='m_-3593417453714472844bodyText-10' style='font-family:'Inter',sans-serif;font-size:15px;line-height:150%;color:#000'>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>PHP is used on a staggering 80% of the top 10 million websites in the world. Start your jouney to becoming a PHP ninja by reading this book.<br></p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>Happy learning!<br></p>
                                                <p style='margin-top:0px;margin-bottom:0px;line-height:150%'>Dianne&nbsp;from SitePoint</p>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%' style='border-top:1px solid #ededf3;border-collapse:initial'>
                                            <tbody><tr>
                                              <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='30' class='m_-3593417453714472844spacingHeight-30' style='line-height:30px;min-height:30px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='left' style='font-family:'Inter',sans-serif;font-size:14px;font-weight:700;line-height:150%;color:#111111'>SitePoint</td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='10'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='center'>
                                                <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='left' width='267' style='width:267px;min-width:267px' class='m_-3593417453714472844mlContentTable m_-3593417453714472844marginBottom'>
                                                  <tbody><tr>
                                                    <td align='left' id='m_-3593417453714472844footerText-14' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <p style='margin-top:0px;margin-bottom:0px'>10-20 Gwynne Street, Cremorne<br>Australia</p>
                                                    </td>
                                                  </tr>
                                                  <tr>
                                                    <td height='25' class='m_-3593417453714472844spacingHeight-20'></td>
                                                  </tr>
                                                  <tr>
                                                    <td align='center'>
                                                      <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='left'>
                                                        <tbody><tr>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NDgmZD1tNG85ZDZ2.ETNSWAj32nzLlydWkAjQxVhHIFre6SHCvFF93MonZ4I' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NDgmZD1tNG85ZDZ2.ETNSWAj32nzLlydWkAjQxVhHIFre6SHCvFF93MonZ4I&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw1HrySKKiwcwPsZDlOL2-gc'>
<img width='24' alt='discord' src='https://ci3.googleusercontent.com/proxy/I804m_5Cgy6ErPHkf0_8I1vlioZocc2bjML7BHQBtxqxS9uT3OOcdgss2v-DMG0nf9goACa2EDbFFPcjl610ykmMn2IIvUvYs68xa7D1Cu_w8EPgY5mU49SX=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/discord.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTQmZD1pMms0aDZo.FknUpJ2PcPjYofuO2y4dyRPlHzXYVeGMjBdbnMJHEPQ' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTQmZD1pMms0aDZo.FknUpJ2PcPjYofuO2y4dyRPlHzXYVeGMjBdbnMJHEPQ&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw24J8BNeSO7DXRHqf10hSKo'>
<img width='24' alt='twitter' src='https://ci5.googleusercontent.com/proxy/8-p7u4CE9SAlowSzP8fzBYe1zWlja-iVGe_nvhIIizFQE-u5jGgSafruMS6eBLyFJYVsijDZfpryFAjkcXARz-X2KIqYTM_PAuGiguHRu-luZAORo75QDZ0s=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/twitter.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTcmZD1kNmY4ZDNn.pPnurbodoHQEajzISfcFCD1KNMAAnL7Oflz-Rk252jA' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTcmZD1kNmY4ZDNn.pPnurbodoHQEajzISfcFCD1KNMAAnL7Oflz-Rk252jA&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw1c2iDfnKHU7IFk4qKNMzCN'>
<img width='24' alt='facebook' src='https://ci6.googleusercontent.com/proxy/rbCPdi0uPTL_Ax_mjpL2419JKMEVYlYNR7_OsiCVveoOIJkQogebSzU3wScRtQiQuXWPa5p-6NY4xa6EstDQqWhDImuFQuKCFrZ1WcqLb_yTYZQNLSfMUtfJ8Q=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/facebook.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                        </tr>
                                                      </tbody></table>
                                                    </td>
                                                  </tr>
                                                </tbody></table>
                                                <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='right' width='267' style='width:267px;min-width:267px' class='m_-3593417453714472844mlContentTable'>
                                                  <tbody><tr>
                                                    <td align='right' id='m_-3593417453714472844footerUnsubscribeText-14' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <p style='margin-top:0px;margin-bottom:0px'>You received this email because you signed up on our website or made purchase from us.</p>
                                                    </td>
                                                  </tr>
                                                  <tr>
                                                    <td height='10'></td>
                                                  </tr>
                                                  <tr>
                                                    <td align='right' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzI5OTAmZD1nNmIycTll.A4Jxh6myizJ2YhjeH9wtmyalZcTPgcPCtdlhVFMVCgQ' style='color:#111111;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzI5OTAmZD1nNmIycTll.A4Jxh6myizJ2YhjeH9wtmyalZcTPgcPCtdlhVFMVCgQ&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0BsXWkeIm7fynqkV7s_C_m'>
<span style='color:#111111'>Unsubscribe</span>
</a>
                                                    </td>
                                                  </tr>
                                                </tbody></table>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='40' class='m_-3593417453714472844spacingHeight-40' style='line-height:40px;min-height:40px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                      </td>
                    </tr>
                  </tbody>
</div>";

            await _emailSender.SendEmailAsync(user.Email, "Welcome To Games Store",
             html);
            await _userManager.AddToRoleAsync(user, "Trainer");
            return Ok(new { Status = "Success", Message = "User created successfully!", user, trainer });

        }

        [HttpGet]
        public async Task<IActionResult> GetSectionsList()
        {
            var sectionList = await _context.Sections.Where(c=>c.IsActive==true).ToListAsync();
            return Ok(new { sectionList });
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionByID([FromQuery] int SectionID)
        {
            var Section = await _context.Sections.FirstOrDefaultAsync(c => c.SectionId == SectionID);
            return Ok(new { Section });
        }


        [HttpGet]
        public async Task<IActionResult> GetTrainersListBySection([FromQuery] int CountryId, [FromQuery] int SectionId)
        {
            var trainerList = _context.Trainers.Where(c => c.CountryId == CountryId && c.SectionId == SectionId).Select(i => new
            {
                i.TrainerId,
                i.FullNameAr,
                i.FullNameEn,
                i.Email,
                i.Mobile,
                i.Tele,
                i.Fax,
                i.GenderId,
                i.Country.CountryTlAr,
                i.Country.CountryTlEn,
                i.Country.CountryPic,
                i.CountryId,
                i.Pic,
                i.Section.SectionTlAr,
                i.Section.SectionTlEn,
                i.SectionId,
                i.DescriptionEn,
                i.DescriptionAr,
                Courses = i.Courses.Select(j => new
                {
                    j.CourseId,
                    j.CourseTlAr,
                    j.CourseTlEn,
                    j.TrainerId,
                    j.CourseTargetId,
                    j.CourseTarget.CourseTargetTlAr,
                    j.CourseTarget.CourseTargetTlEn,
                    j.CourseDescAr,
                    j.CourseDescEn,
                    j.IsActive,
                    j.PublishDate,
                    j.Pic,

                    CourseImage = j.CourseImages.ToList()

                })

            });
            return Ok(new { trainerList });
        }



        [HttpGet]
        public async Task<IActionResult> GetTrainersList([FromQuery] int CountryId)
        {
            var trainerList = _context.Trainers.Where(c => c.CountryId == CountryId).Select(i => new
            {
                i.TrainerId,
                i.FullNameAr,
                i.FullNameEn,
                i.Email,
                i.Mobile,
                i.Tele,
                i.Fax,
                i.GenderId,
                i.Country.CountryTlAr,
                i.Country.CountryTlEn,
                i.Country.CountryPic,
                i.CountryId,
                i.Pic,
                i.Section.SectionTlAr,
                i.Section.SectionTlEn,
                i.SectionId,
                i.DescriptionAr,
                i.DescriptionEn,
                Courses = i.Courses.Select(j => new
                {
                    j.CourseId,
                    j.CourseTlAr,
                    j.CourseTlEn,
                    j.TrainerId,
                    j.CourseTargetId,
                    j.CourseTarget.CourseTargetTlAr,
                    j.CourseTarget.CourseTargetTlEn,
                    j.CourseDescAr,
                    j.CourseDescEn,
                    j.IsActive,
                    j.PublishDate,
                    j.Pic,
                    CourseImage = j.CourseImages.ToList()
                })

            });
            return Ok(new { trainerList });
        }


        [HttpGet]
        public async Task<IActionResult> GetTrainerById([FromQuery] int TrainerId)
        {
            var Trainer = _context.Trainers.Select(

                i => new
                {
                    i.TrainerId,
                    i.FullNameAr,
                    i.FullNameEn,
                    i.Email,
                    i.Mobile,
                    i.Tele,
                    i.Fax,
                    i.GenderId,
                    i.Country.CountryTlAr,
                    i.Country.CountryTlEn,
                    i.CountryId,
                    i.Pic,
                    i.Section.SectionTlAr,
                    i.Section.SectionTlEn,
                    i.SectionId,
                    i.DescriptionAr,
                    i.DescriptionEn,
                    Courses = i.Courses.Select(j => new
                    {
                        j.CourseId,
                        j.CourseTlAr,
                        j.CourseTlEn,
                        j.TrainerId,
                        j.CourseTargetId,
                        j.CourseTarget.CourseTargetTlAr,
                        j.CourseTarget.CourseTargetTlEn,
                        j.CourseDescAr,
                        j.CourseDescEn,
                        j.IsActive,
                        j.PublishDate,
                        j.Pic,
                        CourseImage = j.CourseImages.ToList()

                    })

                }).FirstOrDefault(c => c.TrainerId == TrainerId);
            return Ok(new { Trainer });
        }


        [HttpGet]
        public async Task<IActionResult> GetCourseById([FromQuery] int CourseId)
        {
            var Course = _context.Courses.Select(

                i => new
                {
                    i.CourseId,
                    i.CourseTlAr,
                    i.CourseTlEn,
                    i.TrainerId,
                    i.Trainer,

                    i.CourseTargetId,
                    i.CourseTarget.CourseTargetTlAr,
                    i.CourseTarget.CourseTargetTlEn,
                    i.CourseDescAr,
                    i.CourseDescEn,
                    i.IsActive,
                    i.PublishDate,
                    i.Pic,

                    CourseImage = i.CourseImages.ToList()



                }).FirstOrDefault(c => c.CourseId == CourseId);
            return Ok(new { Course });
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseTarget()
        {
            var courseTargetList = await _context.CourseTargets.Where(c => c.IsActive == true).ToListAsync();

            return Ok(new { courseTargetList });
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByTrainerId([FromQuery] int TrainerId)
        {
            var Course = _context.Courses.Where(c => c.TrainerId == TrainerId).Select(

                i => new
                {
                    i.CourseId,
                    i.CourseTlAr,
                    i.CourseTlEn,
                    i.TrainerId,
                    i.Trainer.FullNameAr,
                    i.Trainer.FullNameEn,
                    i.CourseTargetId,
                    i.CourseTarget.CourseTargetTlAr,
                    i.CourseTarget.CourseTargetTlEn,
                    i.CourseDescAr,
                    i.CourseDescEn,
                    i.IsActive,
                    i.PublishDate,
                    i.Pic,
                    CourseImage = i.CourseImages.ToList()

                });
            return Ok(new { Course });
        }


        [HttpGet]
        public async Task<IActionResult> GetCountriesList()
        {
            var countriesList = await _context.Countries.Where(c => c.CountryIsActive == true).ToListAsync();
            return Ok(new { countriesList });

        }
        [HttpGet]
        public async Task<IActionResult> GetGenderList()
        {
            var genderList = await _context.Genders.ToListAsync();

            return Ok(new { genderList });
        }
        /*************************/

        [HttpGet]
        public async Task<IActionResult> GetIntroImage()
        {
            var sectionList = await _context.Sections.ToListAsync();

            return Ok(new { sectionList });
        }





        [HttpGet]
        public async Task<IActionResult> GetCampList([FromQuery] int CountryId)
        {
            var campList = _context.Camps.Where(c => c.CountryId == CountryId&&c.IsActive==true).Select(i => new
            {
                i.CampId,
                i.CampTlAr,
                i.CampTlEn,
                i.CampDescAr,
                i.CampDescEn,
                i.Url,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.CampTypeId,
                i.CountryId,
                i.CampTargetId,
                i.CampType.CampTypeTlAr,
                i.CampType.CampTypeTlEn,
                i.CampTarget.CampTargetTlAr,
                i.CampTarget.CampTargetTlEn,
                CampImage = i.CampImages,

            });


            return Ok(new { campList });
        }

        [HttpGet]
        public async Task<IActionResult> GetCampTypeList()
        {
            var campTypeList = await _context.CampTypes.Where(c=>c.IsActive==true).ToListAsync();
            return Ok(new { campTypeList });
        }

        [HttpGet]
        public async Task<IActionResult> GetCampTargetList()
        {
            var campTargetList = await _context.CampTargets.Where(c=>c.IsActive==true).ToListAsync();
            return Ok(new { campTargetList });
        }
        [HttpGet]
        public async Task<IActionResult> GetCampByCampId([FromQuery] int CampId)
        {
            var campDetails = _context.Camps.Where(c => c.CampId == CampId).Select(i => new
            {
                i.CampId,
                i.CampTlAr,
                i.CampTlEn,
                i.CampDescAr,
                i.CampDescEn,
                i.Url,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.CampTypeId,
                i.CountryId,
                i.CampTargetId,
                i.CampType.CampTypeTlAr,
                i.CampType.CampTypeTlEn,
                i.CampTarget.CampTargetTlAr,
                i.CampTarget.CampTargetTlEn,
                CampImage = i.CampImages,

            }).FirstOrDefault();
            return Ok(new { campDetails });
        }



        [HttpGet]
        public async Task<IActionResult> GetTournamentList([FromQuery] int CountryId)
        {
            var tournamentList = _context.Tournaments.Where(c => c.CountryId == CountryId&&c.IsActive==true).Select(i => new
            {
                i.TournamentId,
                i.TournamentTlAr,
                i.TournamentTlEn,
                i.TournamentDescAr,
                i.TournamentDescEn,
                i.Url,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.TournamentTypeId,
                i.CountryId,
                i.TournamentTargetId,
                i.TournamentType.TournamentTypeTlAr,
                i.TournamentType.TournamentTypeTlEn,
                i.TournamentTarget.TournamentTargetTlAr,
                i.TournamentTarget.TournamentTargetTlEn,
                TournamentImage = i.TournamentImages,

            });


            return Ok(new { tournamentList });
        }

        [HttpGet]
        public async Task<IActionResult> GetTournamentTypeList()
        {
            var tournamentTypeList = await _context.TournamentTypes.Where(c => c.IsActive == true).ToListAsync();
            return Ok(new { tournamentTypeList });
        }

        [HttpGet]
        public async Task<IActionResult> GetTournamentTargetList()
        {
            var tournamentTargetList = await _context.TournamentTargets.Where(c => c.IsActive == true).ToListAsync();
            return Ok(new { tournamentTargetList });
        }
        [HttpGet]
        public async Task<IActionResult> GetTournamentByTournamentId([FromQuery] int TournamentId)
        {


            var tournamentDetails = _context.Tournaments.Where(c => c.TournamentId == TournamentId).Select(i => new
            {
                i.TournamentId,
                i.TournamentTlAr,
                i.TournamentTlEn,
                i.TournamentDescAr,
                i.TournamentDescEn,
                i.Url,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.TournamentTypeId,
                i.CountryId,
                i.TournamentTargetId,
                i.TournamentType.TournamentTypeTlAr,
                i.TournamentType.TournamentTypeTlEn,
                i.TournamentTarget.TournamentTargetTlAr,
                i.TournamentTarget.TournamentTargetTlEn,
                TournamentImage = i.TournamentImages,

            }).FirstOrDefault();
            return Ok(new { tournamentDetails });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new { status = false, Message = "model not Valid" });


                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(new { status = false, Message = "User not found" });

                }
                var Result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!Result.Succeeded)
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return Ok(new { status = false, message = ModelState });

                }

                return Ok(new { status = true, Message = "Password Changed" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }

        [HttpGet]
        public async Task<IActionResult> GetBannerList()
        {
            var bannerList = await _context.Banners.Where(c => c.BannerIsActive == true).ToListAsync();
            return Ok(new { bannerList });
        }
        [HttpGet]
        public async Task<IActionResult> GetAdzList([FromQuery] int CountryId)
        {
            var adzList = await _context.Adzs.Where(c => c.CountryId == CountryId&&c.AdzIsActive==true).ToListAsync();
            return Ok(new { adzList });
        }
        [HttpPost]

        public async Task<IActionResult> AddCourse([FromBody] CourseVM model)

        {

            var course = new Course();
            course.CourseDescAr = model.CourseDescAr;
            course.CourseDescEn = model.CourseDescEn;
            course.TrainerId = model.TrainerId;
            course.CourseTargetId = model.CourseTargetId;
            course.CourseTlAr = model.CourseTlAr;
            course.CourseTlEn = model.CourseTlEn;
            course.IsActive = model.IsActive;
            course.PublishDate = model.PublishDate;
            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Course");

            if (model.Pic != null)
            {
                var bytes = Convert.FromBase64String(model.Pic);
                string uniqePictureName = Guid.NewGuid() + ".jpeg";
                string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                course.Pic = uniqePictureName;
            }

            await _context.Courses.AddAsync(course);
            _context.SaveChanges();

            if (course.CourseId > 0)
            {
                var ListImage = new List<CourseImage>();

                foreach (var item in model.CourseImage)
                {
                    var ins = new CourseImage();
                    var bytes = Convert.FromBase64String(item);
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    ins.Pic = uniqePictureName;
                    ins.CourseId = course.CourseId;
                    ListImage.Add(ins);
                }
                _context.CourseImages.AddRange(ListImage);
                _context.SaveChanges();

                return Ok(new { status = true, message = "Course added successfully" });

            }



            return Ok(new { status = false, message = "course not add please try again" });

        }

        [HttpGet]
        public async Task<IActionResult> GetSystemConfiguration()
        {
            var SystemConfiguration = await _context.Configurations.FirstOrDefaultAsync();
            return Ok(new { SystemConfiguration });
        }
        [HttpGet]
        public async Task<IActionResult> GetFAQList()
        {
            var SystemConfiguration = await _context.Faqs.ToListAsync();
            return Ok(new { SystemConfiguration });
        }
        [HttpGet]
        public async Task<IActionResult> GetPageContent([FromQuery] int PageContentId)
        {
            var SystemConfiguration = await _context.PageContents.FirstOrDefaultAsync(c => c.PageContentId == PageContentId);
            return Ok(new { SystemConfiguration });
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs([FromBody] ContactU Model)
        {
            Model.TransDate = DateTime.Now;
            await _context.ContactUs.AddAsync(Model);
            _context.SaveChanges();
            return Ok(new { Message = "Message Sent Successfully" });
        }


        [HttpGet]
        public async Task<IActionResult> GetPlansList([FromQuery] int CountryId)
        {
            var planList = await _context.TrainerPlans.Where(c => c.CountryId == CountryId&&c.IsActive==true).ToListAsync();
            return Ok(new { planList });
        }

        [HttpPost]
        public async Task<IActionResult> AddTrainerSubscription([FromBody] TrainerSubscription model)

        {
            var plan = _context.TrainerPlans.Find(model.TrainerPlanId);
            if (plan == null)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = false, Message = "Plan not Found" });

            }
            var Trainer = _context.Trainers.Find(model.TrainerId);
            if (Trainer == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = false, Message = "Trainer not Found" });

            }
            model.Price = plan.Price;
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
            _context.TrainerSubscriptions.Add(model);
            _context.SaveChanges();
            return Ok(new { Status = true, Message = "Subscription Added" });

        }


        [HttpGet]
        public IActionResult AddTrainerDevice([FromQuery] int trainerId, [FromQuery] string deviceId, [FromQuery] bool IsAndroiodDevice)
        {

            try
            {
                var trainer = _context.Trainers.Find(trainerId);
                if (trainer == null)
                {
                    return Ok(new { status = false, message = "trainer not Found" });
                }

                var deviceExists = _context.TrainerDevices.Any(c => c.TrainerId == trainerId && c.DeviceId == deviceId);
                if (deviceExists)
                {
                    return Ok(new { status = false, message = "device already exists" });
                }
                var model = new TrainerDevice
                {
                    TrainerId = trainerId,
                    DeviceId = deviceId,
                    IsAndroiodDevice = IsAndroiodDevice,

                };
                _context.TrainerDevices.Add(model);
                _context.SaveChanges();
                return Ok(new { status = true, message = "deviceId Added To trainer " });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpGet]
        public IActionResult DeleteTrainerDevice([FromQuery] int trainerId, [FromQuery] string deviceId)
        {

            try
            {


                var trainerDevice = _context.TrainerDevices.FirstOrDefault(c => c.TrainerId == trainerId && c.DeviceId == deviceId);
                if (trainerDevice == null)
                {
                    return Ok(new { status = false, message = "device Not Found" });
                }

                _context.TrainerDevices.Remove(trainerDevice);
                _context.SaveChanges();
                return Ok(new { status = true, message = "trainerDevice deleted" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }

        [HttpGet]
        public IActionResult MakeNotificationIsRead([FromQuery] int PublicNotificationDeviceId)
        {

            try
            {
                var model = _context.PublicNotificationDevices.Find(PublicNotificationDeviceId);
                if (model == null)
                {
                    return Ok(new { status = false, message = "Notification not Found" });


                }
                model.IsRead = true;
                _context.PublicNotificationDevices.Update(model);
                _context.SaveChanges();
                
                return Ok(new { status = true, message = "deviceId Added To trainer " });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpPost]
        public IActionResult AddPublicDevice([FromBody] PublicDevice model)
        {

            try
            {
                var publicDevice = _context.PublicDevices.FirstOrDefault(c=>c.DeviceId== model.DeviceId);
                if (publicDevice != null)
                {
                    publicDevice.CountryId = model.CountryId;
                    publicDevice.IsAndroiodDevice = model.IsAndroiodDevice;

                    _context.PublicDevices.Update(publicDevice);
                    _context.SaveChanges();
                    return Ok(new { status = true, message = "deviceId edited" });
                }
                _context.PublicDevices.Add(model);
                _context.SaveChanges();
                return Ok(new { status = true, message = "deviceId Added" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex });
            }
        }

        [HttpGet]
        public IActionResult GetPublicNotificationByDeviceId([FromQuery] string deviceId)
        {
            try
            {
                var publicDevice = _context.PublicDevices.FirstOrDefault(c => c.DeviceId == deviceId);
                var List = _context.PublicNotificationDevices.Include(c => c.PublicNotification).Where(c => c.PublicDeviceId == publicDevice.PublicDeviceId);

                
                return Ok(new { status = true, List });
            }
            catch (Exception )
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
          

        }



      

    }
}


