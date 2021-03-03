using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using HomeHelper.Repositories.MqttSubscriptionMessageRepo;

namespace HomeHelper.Repositories.MqttSubscriptioneMessageRepo
{
    public class MqttSubscriptionMessageRepository : BaseRepository<MqttSubscriptionMessage>, IMqttSubscriptionMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public MqttSubscriptionMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MqttSubscriptionMessage mqttSubscribeMessage)
        {
            _db.Update(mqttSubscribeMessage);
        }
    }
}
