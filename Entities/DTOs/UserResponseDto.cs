using Core.Entities;

namespace Entities.DTOs
{
    // Parola hash/salt'ı ASLA istemciye sızdırmayan güvenli kullanıcı çıktısı.
    public class UserResponseDto : IDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int CustomerId { get; set; }
    }
}
