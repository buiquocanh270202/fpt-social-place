using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.ShareGroupPostCommand
{
    public class ShareGroupPostCommand : ICommand<ShareGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? SharedToUserId { get; set; }
        public Guid? UserSharedId {  get; set; }
        public Guid? GroupId { get; set; }
    }
}
