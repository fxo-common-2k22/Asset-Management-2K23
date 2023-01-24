using FAPP.Areas.AM.ViewModels;
using FAPP.Model;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using PrimaryDataViewModel = FAPP.Areas.AM.ViewModels.PrimaryDataViewModel;
namespace FAPP.Areas.AM.PrimaryData
{
    public class TransportPrimaryData
    {
        public OneDbContext db = null;

        public TransportPrimaryData(OneDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void ImportAll(ref PrimaryDataViewModel vm)
        {
            try
            {
                GetOldZones(ref vm);
                ImportZones(ref vm);
                GetOldVillages(ref vm);
                ImportVillages(ref vm);
                GetOldDrivers(ref vm);
                ImportDrivers(ref vm);
                GetOldBuses(ref vm);
                ImportBuses(ref vm);
                GetOldVehicles(ref vm);
                ImportVehicles(ref vm);
                var importWizard = db.ImportWizard.FirstOrDefault();
                if (importWizard == null)
                {
                    importWizard = new ImportWizard
                    {
                        TransportImported = true,
                    };
                    db.ImportWizard.Add(importWizard);
                }
                else
                {
                    importWizard.TransportImported = true;
                }
                db.SaveChanges();
            }
            catch (Exception x)
            {
                vm.Error += x.GetExceptionMessages();
            }
        }

        public void GetOldZones(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Transport.Zones";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    //vm.TransportZones = DataReaderMapToList<PrimaryDataViewModel.OldTransportZone>(reader)
                    //    .Select(s => new Zones2
                    //    {
                    //        ZoneId = s.ZoneId,
                    //        ZoneName = s.ZoneName,
                    //        Description = s.Description,
                    //        TransportFee = s.TransportFee,
                    //        Discount = s.Discount,
                    //        ModifiedBy = s.ModifiedBy,
                    //        IP = s.IP,
                    //        BranchId = s.BranchId,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportZones(ref PrimaryDataViewModel vm)
        {
            //db.Zones2.AddOrUpdate(s => s.ZoneId, vm.TransportZones.ToArray());
            //db.SaveChanges();
            //vm.Messages += $"{vm.TransportZones.Count()} Zones imported successfully.";
        }

        public void GetOldVillages(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Transport.Villages";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    //vm.TransportVillages = DataReaderMapToList<PrimaryDataViewModel.OldTransportVillage>(reader)
                    //    .Select(s => new Village
                    //    {
                    //        VillageId = s.VillageId,
                    //        VillageName = s.VillageName,
                    //        Description = s.Description,
                    //        ZoneId = s.ZoneId,
                    //        ModifiedBy = s.ModifiedBy,
                    //        IP = s.IP,
                    //        BranchId = s.BranchId,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportVillages(ref PrimaryDataViewModel vm)
        {
            // db.Villages.AddOrUpdate(s => s.VillageId, vm.TransportVillages.ToArray());
            db.SaveChanges();
            // vm.Messages += $"{vm.TransportVillages.Count()} Villages imported successfully.";
        }

        public void GetOldDrivers(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Transport.Drivers";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    //vm.TransportDrivers = DataReaderMapToList<PrimaryDataViewModel.OldTransportDriver>(reader)
                    //    .Select(s => new Driver1
                    //    {
                    //        DriverId = s.DriverId,
                    //        DriverName = s.DriverName,
                    //        IDCardNo = s.IDCardNo,
                    //        LicenseNo = s.LicenseNo,
                    //        Photo = s.Photo,
                    //        Mobile = s.Mobile,
                    //        Active = s.Active,
                    //        HouseNo = s.HouseNo,
                    //        Address = s.Address,
                    //        BranchId = s.BranchId,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportDrivers(ref PrimaryDataViewModel vm)
        {
          //  db.Drivers1.AddOrUpdate(s => s.DriverId, vm.TransportDrivers.ToArray());
            db.SaveChanges();
          //  vm.Messages += $"{vm.TransportDrivers.Count()} Drivers imported successfully.";
        }

        public void GetOldBuses(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Transport.Buses";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    //vm.TransportBuses = DataReaderMapToList<PrimaryDataViewModel.OldTransportBus>(reader)
                    //    .Select(s => new Bus
                    //    {
                    //        BusId = s.BusId,
                    //        BusName = s.BusName,
                    //        Description = s.Description,
                    //        ModifiedBy = s.ModifiedBy,
                    //        IP = s.IP,
                    //        VanNo = s.VanNo,
                    //        Prev__ID = s.Prev__ID,
                    //        DriverName = s.DriverName,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportBuses(ref PrimaryDataViewModel vm)
        {
          //  db.Buses.AddOrUpdate(s => s.BusId, vm.TransportBuses.ToArray());
            db.SaveChanges();
           // vm.Messages += $"{vm.TransportBuses.Count()} Buses imported successfully.";
        }

        public void GetOldVehicles(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Transport.Vehicles";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    var data = DataReaderMapToList<PrimaryDataViewModel.OldTransportVehicle>(reader);
                    //var vehicleTypes = data
                    //    .Select(s => new VehicleType1
                    //    {
                    //        VehicleTypeName = s.VehicleType,
                    //    });

                    //  db.TransportVehicleTypes.AddRange(vehicleTypes);
                    db.SaveChanges();
                    //vm.TransportVehicles = data
                    //    .Select(s => new Vehicles2
                    //    {
                    //        VehicleId = s.VehicleId,
                    //        VehicleTypeId = vehicleTypes.Where(x => x.VehicleTypeName == s.VehicleType).Select(x => x.VehicleTypeId).FirstOrDefault(),
                    //        VechileRegNo = s.VechileRegNo,
                    //        DriverId = s.DriverId,
                    //        BranchId = s.BranchId,
                    //        SeatingCapacity = s.SeatingCapacity,
                    //        OccupiedSeats = s.OccupiedSeats,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportVehicles(ref PrimaryDataViewModel vm)
        {
            // db.Vehicles2.AddOrUpdate(s => s.VehicleId, vm.TransportVehicles.ToArray());
            db.SaveChanges();
            //   vm.Messages += $"{vm.TransportVehicles.Count()} Vehicles imported successfully.";
        }

        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}