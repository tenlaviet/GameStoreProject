using System;
using System.ComponentModel.DataAnnotations;

namespace AspMVC.Areas.Identity.Models.ManageViewModels
{
    public class EditExtraProfileModel
    {
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Địa chỉ email")]
        public string UserEmail { get; set; }
        [Display(Name = "Tên người dùng")]
        [StringLength(100)]
        public string RealName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống")]
        [Display(Name = "Địa chỉ")]
        [StringLength(400)]
        public string HomeAdress { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? UploadAvatar { get; set; }
        public string? AvatarRelativePath { get; set; }
    }
}