using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Enums
{
    public enum NotificationTypeEnum
    {
        System = 0,         // Sistem mesajları
        SocialInteraction = 1, // Beğeni, Yorum
        CommunityInvite = 2,   // Davet
        EducationUpdate = 3,   // Eğitim duyurusu
        BadgeEarned = 4        // Rozet kazanımı
    }
}
