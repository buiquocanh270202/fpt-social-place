using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum StatusCodeEnum
    {
        Success = 0,
        [Description("System Error")]
        Error,
        [Description("Notifications Type not found")]
        NT01_Not_Found,
        [Description("You don't have any notifications")]
        N01_Not_Found,
        [Description("User not found")]
        U01_Not_Found,
        [Description("")]
        Context_Not_Found,
        [Description("User is locked")]
        U02_Lock_User,
        [Description("User is Existed")]
        U03_User_Exist,
        [Description("Can not create user with Email and FeId is null")]
        U04_Can_Not_Create,
        [Description("User don't have any web affilication")]
        U05_Not_Has_WebAffilication,
        [Description("Account is not active")]
        U06_User_Not_Active,
        [Description("Gender is existed in data")]
        G01_Gender_Existed,
        [Description("Gender is not found")]
        G02_Gender_Not_Found,
        [Description("Status not found")]
        ST01_Status_Not_Found,
        [Description("Status is existed")]
        ST02_Status_Existed,
        [Description("Relationship is existed")]
        RL01_Relationship_Existed,
        [Description("Setting is existed")]
        S01_Settings_Existed,
        [Description("Role is existed")]
        R01_Role_Existed,
        [Description("Interest is existed")]
        IT01_Interest_Existed,
        [Description("Post not found")]
        P01_Not_Found,
        [Description("Can not send friend")]
        FR01_Cannot_Send,
        [Description("Accept friend")]
        FR02_Accept_Friend,
        [Description("Cancle friend")]
        FR03_Cancle_Friend,
        [Description("Reject friend")]
        FR04_Reject_Friend,
        [Description("Both are not friend")]
        FR05_Not_Friend,
        [Description("You can not accept friend")]
        FR06_Can_Not_Friend,
        [Description("You can not cancel friend request")]
        FR07_Can_Not_Cancel_Friend,
        [Description("You can not reject friend request")]
        FR08_Can_Not_Reject_Friend,
        [Description("UserRelationship not found")]
        UR01_NOT_FOUND,
        [Description("ReportType is existed")]
        RT01_ReportType_Existed,
        [Description("Group role is existed")]
        GR01_Group_Role_Existed,
        [Description("Group tag is existed")]
        GR02_Group_Tag_Existed,
        [Description("Group setting is existed")]
        GR03_Group_Setting_Existed,
        [Description("Comment can't null")]
        CM01_Comment_Not_Null,
        [Description("Your comment you reply not found")]
        CM02_Parent_Comment_Not_Found,
        [Description("Your comment contain bad word")]
        CM03_Comment_Contain_Bad_Word,
        [Description("Your comment not found")]
        CM04_Comment_Not_Found,
        [Description("Group type is existed")]
        GR04_Group_Type_Existed,
        [Description("Group status is existed")]
        GR05_Group_Status_Existed,
        [Description("Group Name is existed")]
        GR06_Group_Name_Existed,
        [Description("Group Name is not null")]
        GR07_Group_Name_Not_Null,
        [Description("Group is not exist")]
        GR08_Group_Is_Not_Exist,
        [Description("Group post have bad word")]
        GR09_Group_Post_Have_Bad_Word,
        [Description("You can not join this group")]
        GR10_Group_Joined,
        [Description("You are not permission in this group")]
        GR11_Not_Permission,
        [Description("Member is joined or requested in group")]
        GR12_Is_Request,
        [Description("You can not invated this user to join group")]
        GR13_Can_Not_Invated,
        [Description("You can not cancel request to join group")]
        GR14_Can_Not_Cancel_Request,
        [Description("You can not out this group")]
        GR15_Can_Not_Out_Group,
        [Description("User is not exist in this group")]
        GR16_User_Not_Exist_In_Group,
        [Description("Your post have bad word")]
        UP01_Post_Have_Bad_Word,
        [Description("Post not found")]
        UP02_Post_Not_Found,
        [Description("Access denied!")]
        UP03_Not_Authorized,
        [Description("Can't share owner post")]
        UP04_Can_Not_Share_Owner_Post,
        [Description("This account is protected!")]
        PS01_Profile_Status_Private,
        [Description("Request is null!")]
        RQ01_Request_Is_Null,
        [Description("Group is not exist!")]
        GR17_Group_Is_Not_Exist,
        [Description("Yon can not see this content!")]
        UP05_Can_Not_See_Content,
        [Description("User is existed in chat!")]
        UC01_User_Chat_Is_Exist,
        [Description("User is not existed in chat!")]
        UC02_User_Chat_Is_Not_Exist,
        [Description("Invalid report type!")]
        PR01_Invalid_Report_Type,
        [Description("Report not found!")]
        RP01_Report_Not_Found,
        [Description("Can not send message with user!")]
        CH01_Can_Not_Chat,
        [Description("Email is existed!")]
        RGT01_Existed,
        [Description("Email does not existed!")]
        RGT02_Does_Not_Existed,
        [Description("Can not delete this chat!")]
        CH02_Can_Not_Delete_Chat,
        [Description("Can not update this chat!")]
        CH03_Can_Not_Update_Chat,
    }
}
