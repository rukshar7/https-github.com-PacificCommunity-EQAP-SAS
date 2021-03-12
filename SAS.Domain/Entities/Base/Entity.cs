using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public abstract class Entity
    {
        #region Members

        private Guid _guidId = Guid.NewGuid();
        private DateTime? _createdDate = DateTime.Now;
        private string _createdBy;
        private DateTime? _lastModifiedDate = DateTime.Now;
        private string _lastModifiedBy;

        #endregion

        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        public DateTime? LastModifiedDate
        {
            get { return _lastModifiedDate; }
            set { _lastModifiedDate = value; }
        }
        public string LastModifiedBy
        {
            get { return _lastModifiedBy; }
            set { _lastModifiedBy = value; }
        }
        #endregion

        #region Public Methods

        public virtual void CreateTimestamp()
        {
            _createdDate = DateTime.Now;
            _lastModifiedDate = DateTime.Now;

        }

        public virtual void UpdateTimestamp()
        {
            _lastModifiedDate = DateTime.Now;
        }

        #endregion

    }
}
