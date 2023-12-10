using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Helpers.GeneralFunctions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataBase.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IAppointmentRepository AppointmentRepository { get; private set; }
        public IDiscountRepository DiscountRepository { get; private set; }
        public IExaminationPriceRepository ExaminationPriceRepository { get; private set; }
        public IBookingRepository BookingRepository { get; private set; }

        public IUserBookingTrackingRepository UserBookingTrackingRepository { get; private set; }

        private IDbContextTransaction _transaction;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            AppointmentRepository = new AppointmentRepository(_dbContext, new HelperFunctions());
            DiscountRepository = new DiscountRepository(_dbContext);
            ExaminationPriceRepository = new ExaminationPriceRepository(_dbContext);
            BookingRepository = new BookingRepository(_dbContext);
            UserBookingTrackingRepository = new UserBookingTrackingRepository(_dbContext);

            _transaction = null;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw; // Re-throw the exception to propagate it
            }
            finally
            {
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
