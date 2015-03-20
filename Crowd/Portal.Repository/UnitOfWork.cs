using System;
using System.Configuration;
using System.Data.Common;
using System.Data.EntityClient;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace Portal.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly CrouwdEntities _context = new CrouwdEntities();

        private bool _disposed;
        private GenericRepository<ExperienceLevelType> _ExperienceLevelType;
        private GenericRepository<Feed> _Feed;
        private GenericRepository<FeedType> _FeedType;
        private GenericRepository<Follow> _Follow;
        private GenericRepository<Job> _Job;
        private GenericRepository<Message> _Message;
        private GenericRepository<MessageType> _MessageType;
        private GenericRepository<User> _User;
        private GenericRepository<UserEducation> _UserEducation;
        private GenericRepository<UserEducationCourse> _UserEducationCourse;
        private GenericRepository<UserEmployment> _UserEmployment;
        private GenericRepository<UserEmploymentRecommendation> _UserEmploymentRecommendation;
        private GenericRepository<UserJobApplication> _UserJobApplication;
        private GenericRepository<UserJobFavorite> _UserJobFavorite;
        private GenericRepository<UserSkill> _UserSkill;
        private GenericRepository<JobSkill> _JobSkill;
        private GenericRepository<Notification> _Notification;


        public UnitOfWork()
        {
            var originalConnectionString = ConfigurationManager.ConnectionStrings["CrouwdEntities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            var factory = DbProviderFactories.GetFactory(entityBuilder.Provider);
            var providerBuilder = factory.CreateConnectionStringBuilder();

            providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;
            //providerBuilder.Add("Password", Password);
            entityBuilder.ProviderConnectionString = providerBuilder.ToString();

            //_context.Configuration.LazyLoadingEnabled = false;
            _context = new CrouwdEntities(entityBuilder.ToString());
        }
        public GenericRepository<ExperienceLevelType> ExperienceLevelType
        {
            get
            {
                return _ExperienceLevelType ??
                     (_ExperienceLevelType = new GenericRepository<ExperienceLevelType>(_context));
            }
           
        }
        public GenericRepository<Feed> Feed
        {
            get
            {
                return _Feed ??
                     (_Feed = new GenericRepository<Feed>(_context));
            }

        }

        public GenericRepository<FeedType> FeedType
        {
            get
            {
                return _FeedType ??
                       (_FeedType = new GenericRepository<FeedType>(_context));
            }
        }

        public GenericRepository<Follow> Follow
        {
            get
            {
                return _Follow ??
                     (_Follow = new GenericRepository<Follow>(_context));
            }
          
        }

        public GenericRepository<Job> Job
        {
            get
            {
                return _Job ??
                       (_Job = new GenericRepository<Job>(_context));
            }
        }

        public GenericRepository<Message> Message
        {
            get
            {
                return _Message ??
                       (_Message = new GenericRepository<Message>(_context));
            }
        }

        public GenericRepository<MessageType> MessageType
        {
            get
            {
                return _MessageType ??
                       (_MessageType = new GenericRepository<MessageType>(_context));
            }
        }

        public GenericRepository<User> User
        {
            get
            {
                return _User ??
                     (_User = new GenericRepository<User>(_context));
            }
            
        }

        public GenericRepository<UserEducation> UserEducation
        {
            get
            {
                return _UserEducation ??
                       (_UserEducation = new GenericRepository<UserEducation>(_context));
            }
        }

        public GenericRepository<UserEducationCourse> UserEducationCourse
        {
            get
            {
                return _UserEducationCourse ??
                       (_UserEducationCourse = new GenericRepository<UserEducationCourse>(_context));
            }
        }

        public GenericRepository<UserEmployment> UserEmployment
        {
            get
            {
                return _UserEmployment ??
                     (_UserEmployment = new GenericRepository<UserEmployment>(_context));
            }

        }

        public GenericRepository<UserEmploymentRecommendation> UserEmploymentRecommendation
        {
            get
            {
                return _UserEmploymentRecommendation ??
                       (_UserEmploymentRecommendation = new GenericRepository<UserEmploymentRecommendation>(_context));
            }
        }

        public GenericRepository<UserJobApplication> UserJobApplication
        {
            get
            {
                return _UserJobApplication ??
                       (_UserJobApplication = new GenericRepository<UserJobApplication>(_context));
            }
        }

        public GenericRepository<UserJobFavorite> UserJobFavorite
        {
            get
            {
                return _UserJobFavorite ??
                     (_UserJobFavorite = new GenericRepository<UserJobFavorite>(_context));
            }

        }

        public GenericRepository<UserSkill> UserSkill
        {
            get
            {
                return _UserSkill ??
                     (_UserSkill = new GenericRepository<UserSkill>(_context));
            }

        }

        public GenericRepository<JobSkill> JobSkill
        {
            get
            {
                return _JobSkill ??
                     (_JobSkill = new GenericRepository<JobSkill>(_context));
            }

        } 

        public GenericRepository<Notification> Notification
        {
            get
            {
                return _Notification ??
                    (_Notification = new GenericRepository<Notification>(_context));
            }
        }

        private const string Salt = "RnJlZWRvbTE=";
        private static string _password;
        private static readonly object SyncLock = new object();
       
    

        public void SaveChanges()
        {
            _context.GetValidationErrors();
            _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }

}
