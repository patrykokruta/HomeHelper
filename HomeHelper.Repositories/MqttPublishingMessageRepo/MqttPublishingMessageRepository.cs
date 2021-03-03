using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;

namespace HomeHelper.Repositories.MqttPublishingMessageRepo
{
    public class MqttPublishingMessageRepository : BaseRepository<MqttPublishingMessage>, IMqttPublishingMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public MqttPublishingMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MqttPublishingMessage mqttPublishMessage)
        {
            _db.Update(mqttPublishMessage);
        }
    }
}
