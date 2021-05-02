using AirtableApiClient;
using AutoMapper;
using LogProxy.API.DTOs;
using LogProxy.API.Entities;
using LogProxy.API.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace LogProxy.API.Services
{
    public class MessageService : IMessageService
    {
        private const string _tableName = "Messages";
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MessageService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            var records = new List<AirtableRecord<MessageDTO>>();
            using (var airtableBase = GetAirtableBase())
            {
                var task = airtableBase.ListRecords<MessageDTO>(_tableName);
                var response = await task;
                if (!response.Success) throw new Exception(response.AirtableApiError.ErrorMessage);
                records.AddRange(response.Records.ToList());
            }
            return records.Where(r => r.Fields.Id != null).Select(r => _mapper.Map<Message>(r.Fields)).ToList();
        }

        public async Task<Message> GetByIdAsync(string id)
        {
            AirtableRetrieveRecordResponse<MessageDTO> response;
            using (var airtableBase = GetAirtableBase())
            {
                var task = airtableBase.RetrieveRecord<MessageDTO>(_tableName, id);
                response = await task;
                if (!response.Success)
                {
                    if (response.AirtableApiError.ErrorCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception(response.AirtableApiError.ErrorMessage);
                    }
                }
            }
            return _mapper.Map<Message>(response.Record.Fields);
        }

        public async Task<string> PostAsync(Message message)
        {
            AirtableCreateUpdateReplaceRecordResponse response;
            using (var airtableBase = GetAirtableBase())
            {
                var messageDTO = _mapper.Map<MessageDTO>(message);
                var fields = GetFields(messageDTO);
                var task = airtableBase.CreateRecord(_tableName, fields, true);
                response = await task;
                if (!response.Success) throw new Exception(response.AirtableApiError.ErrorMessage);
            }
            return response.Record.Id;
        }

        private Fields GetFields(MessageDTO messageDTO)
        {
            var fields = new Fields();
            foreach (PropertyInfo pi in messageDTO.GetType().GetProperties())
            {
                fields.AddField(pi.Name, pi.GetValue(messageDTO, null)?.ToString());
            }
            return fields;
        }

        private AirtableBase GetAirtableBase() => new AirtableBase(_configuration["Airtable:APIKey"], _configuration["Airtable:ApplicationId"]);
    }
}