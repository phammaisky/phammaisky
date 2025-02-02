﻿using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Service
{
    // operations you want to expose
    public interface IGadgetService
    {
        IEnumerable<Gadget> GetGadgets();
        IEnumerable<Gadget> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Gadget GetGadget(int id);
        void CreateGadget(Gadget gadget);
        void SaveGadget();
        IEnumerable<Test> GetTests();
    }

    public class GadgetService : IGadgetService
    {
        private readonly IGadgetRepository gadgetsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITestRepository testRepository;
        private readonly IUnitOfWork unitOfWork;

        public GadgetService(IGadgetRepository gadgetsRepository, ICategoryRepository categoryRepository, ITestRepository testRepository, IUnitOfWork unitOfWork)
        {
            this.gadgetsRepository = gadgetsRepository;
            this.categoryRepository = categoryRepository;
            this.testRepository = testRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IGadgetService Members

        public IEnumerable<Gadget> GetGadgets()
        {
            var gadgets = gadgetsRepository.GetAll();
            return gadgets;
        }

        public IEnumerable<Gadget> GetCategoryGadgets(string categoryName, string gadgetName = null)
        {
            var category = categoryRepository.GetCategoryByName(categoryName);
            return category.Gadgets.Where(g => g.Name.ToLower().Contains(gadgetName.ToLower().Trim()));
        }

        public Gadget GetGadget(int id)
        {
            var gadget = gadgetsRepository.GetById(id);
            return gadget;
        }

        public void CreateGadget(Gadget gadget)
        {
            gadgetsRepository.Add(gadget);
        }

        public IEnumerable<Test> GetTests()
        {
            var tests = testRepository.GetAll();
            return tests;
        }

        public void SaveGadget()
        {
            unitOfWork.Commit();
        }

        #endregion
    
    }
}
