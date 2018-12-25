using System.Data.SqlClient;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.BusinessLogicLayer
{
    public class BusinessLogicDbTrx
    {
        DataBaseUtilityUpdated objDbUlility = new DataBaseUtilityUpdated();

        #region Academic Year Master
        public DataTable GetAcademicYearDtl()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetAcademicYearDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region District Master
        public DataTable GetDistrictDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_GetDistrictDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region CircleUserMaster
        public DataTable GetCircleUserMasterDetailsById(Int64 DataUniqueId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleUserMasterDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CercleId", DataUniqueId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateCircleMasterAddressNew(string CircleId, string SchooldAddress, string Pincode, string InspectorName, string InspectorPhoneNo, string InspectorEmailId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateCircleMasterAddressNew");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleId", CircleId);
                cmd.Parameters.AddWithValue("@SchooldAddress", SchooldAddress);
                cmd.Parameters.AddWithValue("@Pincode", Pincode);
                cmd.Parameters.AddWithValue("@InspectorName", InspectorName);
                cmd.Parameters.AddWithValue("@InspectorPhoneNo", InspectorPhoneNo);
                cmd.Parameters.AddWithValue("@InspectorEmailId", InspectorEmailId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateCircleUserMasterAddress(string CircleId, string SchooldAddress, string Pincode, string InspectorName, string InspectorPhoneNo, string InspectorEmailId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateCircleUserMasterAddress");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleId", CircleId);
                cmd.Parameters.AddWithValue("@SchooldAddress", SchooldAddress);
                cmd.Parameters.AddWithValue("@Pincode", Pincode);
                cmd.Parameters.AddWithValue("@InspectorName", InspectorName);
                cmd.Parameters.AddWithValue("@InspectorPhoneNo", InspectorPhoneNo);
                cmd.Parameters.AddWithValue("@InspectorEmailId", InspectorEmailId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetCircleLoginDtl(string circle_user_name, string circle_password)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleLoginDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", circle_user_name);
                    cmd.Parameters.AddWithValue("@UsrPassword", circle_password);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInCericleUserNew(CircleMaster objcust)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateInCericleUserNew");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserUniqueid", objcust.CircleID);
                cmd.Parameters.AddWithValue("@CircleOfficerName", objcust.CircleOfficerName);
                cmd.Parameters.AddWithValue("@MobileNo", objcust.MobileNo);
                cmd.Parameters.AddWithValue("@EmailId", objcust.EmailId);
                cmd.Parameters.AddWithValue("@PoliceStn", objcust.PoliceStation);
                cmd.Parameters.AddWithValue("@CircleAddress", objcust.Address);
                cmd.Parameters.AddWithValue("@CirclePincode", objcust.CirclePinCode);
                cmd.Parameters.AddWithValue("@AlternateMobileno", objcust.AlternateMobileNo);
                cmd.Parameters.AddWithValue("@Active", objcust.Active);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInCericleUser(CircleUser objcust)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_UpdateInCericleUser");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleOfficerName", objcust.CircleOfficerName);
                cmd.Parameters.AddWithValue("@MobileNo", objcust.MobileNo);
                cmd.Parameters.AddWithValue("@EmailId", objcust.EmailId);
                cmd.Parameters.AddWithValue("@CircleAddress", objcust.Address);
                cmd.Parameters.AddWithValue("@CirclePincode", objcust.CirclePinCode);
                cmd.Parameters.AddWithValue("@AlternateMobileno", objcust.AlternateMobileNo);
                cmd.Parameters.AddWithValue("@Active", objcust.active);
                cmd.Parameters.AddWithValue("@UserUniqueid", objcust.CircleUserID);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetCircleUser()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleUserDetails"))
                {
                cmd.CommandType = CommandType.StoredProcedure;
                ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertReqLockStock(string district, string circle, string reqYear, string lockDate, int reqLock, int stockLock)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CircleLock");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@district", district);
                cmd.Parameters.AddWithValue("@circle", circle);
                cmd.Parameters.AddWithValue("@reqyear", reqYear);
                cmd.Parameters.AddWithValue("@lockdate", lockDate);
                cmd.Parameters.AddWithValue("@reqlock", reqLock);
                cmd.Parameters.AddWithValue("@stocklock", stockLock);


                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInMstCircleUser1(CircleUser objCircle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstCircleUserInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_Circleid", objCircle.CircleID);
                cmd.Parameters.AddWithValue("@in_Circleofficername", objCircle.CircleOfficerName);
                cmd.Parameters.AddWithValue("@in_mobileno", objCircle.MobileNo);
                cmd.Parameters.AddWithValue("@in_emailid", objCircle.EmailId);
                cmd.Parameters.AddWithValue("@in_Circleaddress", objCircle.Address);
                cmd.Parameters.AddWithValue("@in_userid", objCircle.Userid);
                cmd.Parameters.AddWithValue("@in_password", objCircle.Password);
                cmd.Parameters.AddWithValue("@in_active", objCircle.active);
                cmd.Parameters.AddWithValue("@in_flag", objCircle.flag);
                cmd.Parameters.AddWithValue("@in_circlepincode", objCircle.CirclePinCode);
                cmd.Parameters.AddWithValue("@in_AlternateMobileno", objCircle.AlternateMobileNo);


                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region Circle Master

        public bool InsertUpdateCircleStockUpdate(string xData, int isConfirmed = 0)
        {
            try
            {
                /*SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "LOAD XML LOCAL INFILE '" + filepath.Replace("\\", "\\\\") + "' INTO TABLE circle_stock_update ROWS IDENTIFIED BY '" + nodename + "' SET CREATED_TS = NOW();";
                objDbUlility.ExNonQuery(cmd);*/

                SqlCommand cmd = new SqlCommand("Sp_CircleStockInsertUpdateBatch");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@xData", xData);
                cmd.Parameters.AddWithValue("@IsConfirmed", isConfirmed);
                cmd.Parameters.Add("xData", SqlDbType.NVarChar);
                cmd.Parameters["xData"].Value = xData;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertUpdateCircleStockUpdateTrnf(string xData, int isConfirmed = 0)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CircleStockInsertUpdateBatchTrnf");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("xData", SqlDbType.NVarChar);
                cmd.Parameters["xData"].Value = xData;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertInMstCircle(MstCircle objCircle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstCircleInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_DistrictId", objCircle.DistrictId);
                cmd.Parameters.AddWithValue("@in_CircleCode", objCircle.CircleCode);
                cmd.Parameters.AddWithValue("@in_CircleName", objCircle.CircleName);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool UpdateInMstCircle(MstCircle objCircle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstCircleUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_CircleId", objCircle.CircleId);
                cmd.Parameters.AddWithValue("@in_CircleCode", objCircle.CircleCode);
                cmd.Parameters.AddWithValue("@in_CircleName", objCircle.CircleName);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public bool DeleteInCircle(int DataUniqueId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_DeleteInCircle");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@In_DataUniqueId", DataUniqueId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public DataTable GetCircleMasterDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetCircleMasterDetailsForDistrict(Int32 DistrictId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleMasterDetailsForDistrict"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DistrictId", DistrictId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable IsCircleRecordExistInRefTable(Int32 DataUniqueId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_IsCircleRecordExistInRefTable"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", DataUniqueId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetCircleMasterDetailsByCircleIdNew(Int32 CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleMasterDetailsByCircleIdNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetCircleMasterDetailsByCircleId(Int32 CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleMasterDetailsByCircleIdNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetCircleDtilById(Int32 CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleDtilById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetCircleDtilByDistId(Int32 DistId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleDtilByIdNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_DistId", DistId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable isDuplicateRecordExistInCircle(int CircleID, string CircleCode)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_isDuplicateRecordExistInCircle"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_CircleId", CircleID);
                    cmd.Parameters.AddWithValue("@In_circleCode", CircleCode);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetCircleLockByCircleId(int CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCircleLockByCircleId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@circleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { }
        }

        #endregion

        #region CustomerOrder No
        public DataTable GetCustomerOrderNoByLanguageAndChallanCatId(int LanguageId, int ChallanCategoryId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetCustomerOrderNo"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inLanguageId", LanguageId);
                    cmd.Parameters.AddWithValue("@inChallanCategoryId", ChallanCategoryId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Transport
        public DataTable GetTransportDtl()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_GetTransportDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertInMstTransport(MstTransporter objTransport)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstTransporterInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_Transport_Name", objTransport.Transporter_name);
                cmd.Parameters.AddWithValue("@in_Transport_address", objTransport.Transporter_address);
                cmd.Parameters.AddWithValue("@in_Transport_Phone_no", objTransport.Transporter_phone_no);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        #endregion

        #region BookMaster
        public DataTable GetBookMasterDetailsById(Int64 CategoryId, Int64 LanguageId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookMasterDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                    cmd.Parameters.AddWithValue("@LanguageId", LanguageId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBookMasterDetailsByIdNew(long SchoolId, long CategoryId, long LanguageId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookMasterDetailsByIdNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                    cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                    cmd.Parameters.AddWithValue("@LanguageId", LanguageId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBookMasterDetailsByLanguageId(Int64 LanguageId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookMasterDetailsByLanguageId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InLanguageId", LanguageId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBookMasterDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBookDtlByChallanCatIdAndLanguageId(Int32 InLanguageId, Int32 InChallanCatId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookDtlByChallanCatIdAndLanguageId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InLanguageId", InLanguageId);
                    cmd.Parameters.AddWithValue("@InChallanCatId", InChallanCatId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBookMaster()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_BookMaster"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool GetLockThisBook(int bookId, bool val)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_LockThisBook");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.Parameters.AddWithValue("@val", val);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        #endregion

        #region Binder
        public bool DeleteBinderAllotment(string ReqId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_DeleteBinderAllotment");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", ReqId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool BinderAllotmentConfirm(BinderAllotQuantity objBinderAllotQuantity, string RequisitionIdsInCommaSeparated)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_BinderAllotmentConfirm");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequisitionIds", RequisitionIdsInCommaSeparated);
                cmd.Parameters.AddWithValue("@InStaus", objBinderAllotQuantity.SaveStatus);
                cmd.Parameters.AddWithValue("@InUserId", objBinderAllotQuantity.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool BinderAllotmentDtlInsert(int allotId, string shortCode, int challanId, string bookCode, int scannedStatus, string userId, string xData)
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("Sp_InsertBinderAllotQtyDetail");
                SqlCommand cmd = new SqlCommand("Sp_InsertBinderAllotQtyDtl");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BinderAllotID", allotId);
                cmd.Parameters.AddWithValue("@BinderShortCode", shortCode);
                cmd.Parameters.AddWithValue("@ChallanID", challanId);
                cmd.Parameters.AddWithValue("@BookCode", bookCode);
                cmd.Parameters.AddWithValue("@ScannedStatus", scannedStatus);
                cmd.Parameters.AddWithValue("@UserID", userId);
                //cmd.Parameters.AddWithValue("@xData", xData);
                cmd.Parameters.Add("xData", SqlDbType.NVarChar);
                cmd.Parameters["xData"].Value = xData;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool AutoCorrectBinderDtlDuplicates(string FromDate = "", string ToDate = "")
        {
            try
            {
                /*SqlCommand cmd = new SqlCommand("usp_AutoCorrectBinderDtlDuplicates");
                cmd.CommandType = CommandType.StoredProcedure;
                objDbUlility.ExNonQuery(cmd);*/
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotmentDtlByBinderId(string BinderAllotCode, int ScnStatus, int BinderAllotId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderQtyDetail"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BinderAllotCode", BinderAllotCode);
                    cmd.Parameters.AddWithValue("@ScnStatus", ScnStatus);
                    cmd.Parameters.AddWithValue("@BinderAllotId", BinderAllotId);
                    cmd.Parameters.AddWithValue("@Opmode", 1);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotmentDtlByBinderIdFrom_To(string BinderAllotCode, int ScnStatus, int BinderAllotId, int From, int To)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderQtyDetail_Filter"))
                {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@From_BinderAllotCode", From);
                cmd.Parameters.AddWithValue("@To_BinderAllotCode", To);
                cmd.Parameters.AddWithValue("@ScnStatus", ScnStatus);
                cmd.Parameters.AddWithValue("@BinderAllotId", BinderAllotId);
                cmd.Parameters.AddWithValue("@Opmode", 1);
                ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBinderAllotmentDtlCountByBinderAllotCode(string BinderAllotCode, int ScnStatus)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderQtyDetail"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BinderAllotCode", BinderAllotCode);
                    cmd.Parameters.AddWithValue("@ScnStatus", ScnStatus);
                    cmd.Parameters.AddWithValue("@Opmode", 0);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotQty()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAllotQty"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInBookAllotQty(BinderAllotQuantity objBookAllotQty, out string reqGenCode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_BinderAllotQtyInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_BINDER_ID", objBookAllotQty.BinderId);
                cmd.Parameters.AddWithValue("@in_BINDER_ALLOT_CODE", objBookAllotQty.AllotmentCode);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objBookAllotQty.LanguageId);
                cmd.Parameters.AddWithValue("@in_CHALLAN_CATEGORY_ID", objBookAllotQty.ChallanCategoryId);
                cmd.Parameters.AddWithValue("@in_BOOK_CODE", objBookAllotQty.BookCode);
                cmd.Parameters.AddWithValue("@in_TOT_QTY", objBookAllotQty.TotQty);
                cmd.Parameters.AddWithValue("@in_LOT", objBookAllotQty.Lot);
                cmd.Parameters.AddWithValue("@in_REQ_QTY", objBookAllotQty.ReqQty);
                cmd.Parameters.AddWithValue("@in_ACAD_YEAR_ID", objBookAllotQty.AcademicYearID);
                cmd.Parameters.AddWithValue("@InUserId", objBookAllotQty.UserId);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInBookAllotQty(BinderAllotQuantity objBookAllotQty)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_BinderAllotQtyUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_ID", objBookAllotQty.ID);
                cmd.Parameters.AddWithValue("@in_TOT_QTY", objBookAllotQty.TotQty);
                cmd.Parameters.AddWithValue("@in_LOT", objBookAllotQty.Lot);
                cmd.Parameters.AddWithValue("@in_REQ_QTY", objBookAllotQty.ReqQty);
                cmd.Parameters.AddWithValue("@InUserId", objBookAllotQty.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderMaster()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                    return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotmentQtyView(DateTime StartDate, DateTime EndDate, Int16 AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAllotmentQtyView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBinderAllotmentQtyByID(Int32 In_AllotmentId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAllotmentQtyByID"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_AllotmentId", In_AllotmentId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataSet GetBinderAllotmentDtlByCode(string InBinderAllotmentCode, Int32 ChallanId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAllotmentDtlByCode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InBinderAllotmentCode", InBinderAllotmentCode);
                    cmd.Parameters.AddWithValue("@InChallanId", ChallanId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool UpdateSchConfirmChallan(SchProvisionalChallan objSchProvisionalChallan)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateSchConfirmChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objSchProvisionalChallan.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objSchProvisionalChallan.ConsigneeNo);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objSchProvisionalChallan.VehicleNo);
                cmd.Parameters.AddWithValue("@InChallanId", objSchProvisionalChallan.ChallanId);
                cmd.Parameters.AddWithValue("@InUserId", objSchProvisionalChallan.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        #endregion

        #region SchoolMaster
        public DataTable GetSchoolMasterDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool DeleteInSchool(Int32 DataUniqueId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_DeleteInSchool");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@In_DataUniqueId", DataUniqueId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchoolMasterDetailsById(Int64 SchoolID, Int64 CircleID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolMasterDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@circleID", CircleID);
                    cmd.Parameters.AddWithValue("@schoolID", SchoolID);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchoolMasterDetailsByCircleId(Int64 CircleID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolMasterDetailsByCircleId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@circleID", CircleID);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchoolMasterDetailsBySchoolId(Int64 SchoolId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolMasterDetailsBySchoolId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable CheckSchoolMasterDetailsBySchoolCode(string SchoolCode, Int64 SchoolId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CheckSchoolMasterDetailsBySchoolCode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolCode", SchoolCode);
                    cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable IsSchoolRecordExistInRefTable(Int32 DataUniqueId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_IsSchoolRecordExistInRefTable"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inDataUniqueId", DataUniqueId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchoolMasterDetailsBySchoolCode(string SchoolCode)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolMasterDetailsBySchoolCode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolCode", SchoolCode);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable isDuplicateRecordExistInSchool(Int32 InDataUniqueId, string InSchoolCode)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_isDuplicateRecordExistInSchool"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InDataUniqueId", InDataUniqueId);
                    cmd.Parameters.AddWithValue("@InSchoolCode", InSchoolCode);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInSchool(MstSchool objcust, out string SchoolId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertInSchool");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleId", objcust.CircleId);
                cmd.Parameters.AddWithValue("@DistrictId", objcust.DistrictID);
                cmd.Parameters.AddWithValue("@SchoolCode", objcust.SchoolCode);
                cmd.Parameters.AddWithValue("@SchoolName", objcust.SchoolName);
                cmd.Parameters.AddWithValue("@SchoolAdrees", objcust.SchoolAdrees);
                cmd.Parameters.AddWithValue("@PostalCode", objcust.PostalCode);
                if (!string.IsNullOrWhiteSpace(objcust.SchoolEmailid))
                {
                    cmd.Parameters.AddWithValue("@SchoolEmailid", objcust.SchoolEmailid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SchoolEmailid", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@SchoolMobile", objcust.SchoolMobile);
                cmd.Parameters.AddWithValue("@SchoolAltMobile", objcust.SchoolAlternateMobile);
                cmd.Parameters.AddWithValue("@DeleivaryAddress", objcust.DeleivaryAddress);
                cmd.Parameters.AddWithValue("@PoliceStation", objcust.PoliceStation);
                cmd.Parameters.AddWithValue("@UserId", objcust.UserId);
                cmd.Parameters.Add("@SchoolId", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                objDbUlility.ExNonQuery(cmd);
                SchoolId = cmd.Parameters["@SchoolId"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool UpdateInSchool(MstSchool objcust)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateInSchool");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@schoolID", objcust.SchoolID);
                cmd.Parameters.AddWithValue("@SchoolCode", objcust.SchoolCode);
                cmd.Parameters.AddWithValue("@SchoolName", objcust.SchoolName);
                cmd.Parameters.AddWithValue("@SchoolAdrees", objcust.SchoolAdrees);
                cmd.Parameters.AddWithValue("@PostalCode", objcust.PostalCode);
                if (!string.IsNullOrWhiteSpace(objcust.SchoolEmailid))
                {
                    cmd.Parameters.AddWithValue("@SchoolEmailid", objcust.SchoolEmailid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SchoolEmailid", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@SchoolMobile", objcust.SchoolMobile);
                cmd.Parameters.AddWithValue("@SchoolAltMobile", objcust.SchoolAlternateMobile);
                cmd.Parameters.AddWithValue("@DeleivaryAddress", objcust.DeleivaryAddress);
                cmd.Parameters.AddWithValue("@PoliceStation", objcust.PoliceStation);
                cmd.Parameters.AddWithValue("@UserId", objcust.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region LanguageMaster
        public DataTable GetLanguageMasterDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetLanguageMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertInMstLanguage(Language objLanguage)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstLanguageInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_Language_Name", objLanguage.language_name);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region ChallanBookCeategory
        public DataTable GetChallanBookCeategory()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanBookCeategory"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInMstCategory(Category objCategory)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstChallanBookCategoryInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_Category_Name", objCategory.Category_name);

                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region BookCategoryMaster
        public DataTable GetBookCategoryMasterDetails()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookCategoryMasterDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInMstBookCategory(Category objCategory)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MstBookCategoryInsert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_Book_Category_Name", objCategory.Category_name);

                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region Requisition

        public DataTable GetRequestYear()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_GetReqYear"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInSchRequisition(SchRequisition objSchRequisition, out string reqGenCode)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objSchRequisition.reqTrxCollection.RemoveAll(r => removeList.Any(a => a == r.StudentEnrolled));

                SqlCommand cmd = new SqlCommand("Sp_Sch_Requisition_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InSchoolID", objSchRequisition.SchoolID);
                cmd.Parameters.AddWithValue("@InCategoryID", objSchRequisition.MstCategory.CategoryID);
                cmd.Parameters.AddWithValue("@InLanguageID", objSchRequisition.MstLanguage.LanguageID);
                cmd.Parameters.AddWithValue("@InStaus", objSchRequisition.SaveStatus);
                cmd.Parameters.AddWithValue("@InAcademicYearID", objSchRequisition.AcademicYearID);
                cmd.Parameters.AddWithValue("@InUserId", objSchRequisition.UserId);
                cmd.Parameters.Add("InTrx_Sch_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                //cmd.Parameters["InTrx_Sch_requisition_dtl_xml"].Value = GenerateToXml(objSchRequisition.reqTrxCollection);
                cmd.Parameters["InTrx_Sch_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<SchRequisitionDtl>(objSchRequisition.reqTrxCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchRequisitionViewDataByCercleId(DateTime StartDate, DateTime EndDate, Int16 CircleID, Int16 AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_Sch_Requisition_View"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    cmd.Parameters.AddWithValue("@In_CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable Sp_SchRequisitionForProbChallan(Int16 CircleID, Int16 AccadYear, Int16 LanguageId, string InBookCode)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchRequisitionForProbChallan"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    cmd.Parameters.AddWithValue("@In_LanguageId", LanguageId);
                    cmd.Parameters.AddWithValue("@In_BookCode", InBookCode);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchRequisitionForProbChallanById(Int32 InChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchRequisitionForProbChallanById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSchBookRequisitionCalculatedDtl(Int16 InLanguageId, string InRequisitionIds, string InBookCodeIds)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchBookRequisitionCalculatedDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InRequisitionIds", InRequisitionIds);
                    cmd.Parameters.AddWithValue("@InBookCodeIds", InBookCodeIds);
                    cmd.Parameters.AddWithValue("@InLanguageId", InLanguageId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchBookDtlByChallanId(Int32 InChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchBookDtlByChallanId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }



        public bool DeleteSchRequisition(string ReqId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Sch_Requisition_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", ReqId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public bool SchProbChallanDelete(Int64 ReqId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_SchProbChallanDelete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", ReqId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSchRequisitionDtlByReqId(Int64 ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchRequisitionDtlByReqId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionDtlByReqIdNew(long ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetRequisitionDtlByReqIdNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionByReqIDNew(long ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetRequisitionByReqIDNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public DataTable GetRequisitionDtlByReqIdSimplifiedNew(long ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetRequisitionDtlByReqIdSimplifiedNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSchRequisitionByReqId(Int64 ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchRequisitionByReqId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_ReqId", ReqId);
                     ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInSchRequisition(SchRequisition objSchRequisition)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objSchRequisition.reqTrxCollection.RemoveAll(r => removeList.Any(a => a == r.StudentEnrolled));

                SqlCommand cmd = new SqlCommand("Sp_Sch_Requisition_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InRequisitionId", objSchRequisition.RequisitionID);
                cmd.Parameters.AddWithValue("@InStaus", objSchRequisition.SaveStatus);
                cmd.Parameters.AddWithValue("@InUserId", objSchRequisition.UserId);
                cmd.Parameters.Add("InTrx_Sch_requisition_dtl_xml", SqlDbType.NVarChar);
                //cmd.Parameters["InTrx_Sch_requisition_dtl_xml"].Value = GenerateToXml(objSchRequisition.reqTrxCollection);
                cmd.Parameters["InTrx_Sch_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<SchRequisitionDtl>(objSchRequisition.reqTrxCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool SchRequisitionConfirm(SchRequisition objSchRequisition, string RequisitionIdsInCommaSeparated)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Sch_Requisition_Confirm");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequisitionIds", RequisitionIdsInCommaSeparated);
                cmd.Parameters.AddWithValue("@InStaus", objSchRequisition.SaveStatus);
                cmd.Parameters.AddWithValue("@InUserId", objSchRequisition.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool RequisitionConfirm(Requisition objRequisition, string RequisitionIdsInCommaSeparated)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Requisition_Confirm");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequisitionIds", RequisitionIdsInCommaSeparated);
                cmd.Parameters.AddWithValue("@InStaus", Convert.ToInt32(!string.IsNullOrEmpty(objRequisition.SaveStatus) ? objRequisition.SaveStatus : default(int).ToString()));
                cmd.Parameters.AddWithValue("@InUserId", objRequisition.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInRequisition(Requisition objcust, out string reqGenCode)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objcust.reqTrxCollection.RemoveAll(r => removeList.Any(a => a == r.StudentEnrolled));

                SqlCommand cmd = new SqlCommand("Sp_Requisition_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@schoolID", objcust.SchoolID);
                cmd.Parameters.AddWithValue("@CircleId", objcust.CircleID);
                cmd.Parameters.AddWithValue("@categoryID", objcust.CategoryID);
                cmd.Parameters.AddWithValue("@languageID", objcust.LanguageID);
                cmd.Parameters.AddWithValue("@UserId", objcust.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<RequisitionTrxDtl>(objcust.reqTrxCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInRequisition(Requisition objcust)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                //objcust.reqTrxCollection.RemoveAll(r => removeList.Any(a => a == r.StudentEnrolled));

                SqlCommand cmd = new SqlCommand("Sp_Requisition_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", objcust.RequisitionID);
                cmd.Parameters.AddWithValue("@CircleId", objcust.CircleID);
                cmd.Parameters.AddWithValue("@schoolID", objcust.SchoolID);
                cmd.Parameters.AddWithValue("@categoryID", objcust.CategoryID);
                cmd.Parameters.AddWithValue("@languageID", objcust.LanguageID);
                cmd.Parameters.AddWithValue("@UserId", objcust.UserId);
                cmd.Parameters.AddWithValue("@SavedStatus", Convert.ToInt32(!string.IsNullOrEmpty(objcust.SaveStatus) ? objcust.SaveStatus : default(int).ToString()));
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<RequisitionTrxDtl>(objcust.reqTrxCollection)).InnerXml;

                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool DeleteRequisition(string ReqId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Requisition_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", ReqId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool DeleteRequisitionNew(string ReqId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Requisition_Delete_New");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqId", ReqId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionViewDataByCercleId(Int64 CircleID, Int64 topLimit)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_Requisition_View"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@topLimit", topLimit);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionViewNew(DateTime StartDate, DateTime EndDate, Int16 CircleID, Int16 AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_Requisition_View_New"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    cmd.Parameters.AddWithValue("@In_CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionViewNewForApproval(DateTime StartDate, DateTime EndDate, int DistrictID, int CircleID, int ApprovalStatus = 0, int AccadYear = 0, int IsForDistApproval = 0, int DistApprovalstatus = 0, int IsForAdminApproval = 0, int AdminApprovalstatus=0)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_Requisition_View_New_ForApproval"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", StartDate);
                    cmd.Parameters.AddWithValue("@In_ToDate", EndDate);
                    cmd.Parameters.AddWithValue("@In_DistrictID", DistrictID);
                    cmd.Parameters.AddWithValue("@In_CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus);//0
                    cmd.Parameters.AddWithValue("@DistApprovalstatus", DistApprovalstatus);//0
                    cmd.Parameters.AddWithValue("@IsForDistApproval", IsForDistApproval);//1
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);

                    if (IsForAdminApproval == 1) // in case of admin login
                    {
                        cmd.Parameters.AddWithValue("@AdminApprovalstatus", AdminApprovalstatus);
                        cmd.Parameters.AddWithValue("@Opmode", 6);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Opmode", default(int));
                    }

                     ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool RequisitionApprovalAtaTime(Requisition objRequisition, string RequisitionIdsInCommaSeparated, int IsForDistApproval = 0)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Requisition_View_New_ForApproval");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequisitionIds", RequisitionIdsInCommaSeparated);
                cmd.Parameters.AddWithValue("@InStaus", IsForDistApproval <= 0 ? (objRequisition.ISAPPROVED > default(int) ? objRequisition.ISAPPROVED : default(int)) : (objRequisition.ISAPPROVED_DIST > default(int) ? objRequisition.ISAPPROVED_DIST : default(int)));
                cmd.Parameters.AddWithValue("@InUserId", objRequisition.UserId);
                cmd.Parameters.AddWithValue("@Opmode", IsForDistApproval <= 0 ? 1 : 3);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool RequisitionApproval(Requisition objRequisition, string RequisitionIdsInCommaSeparated, int IsForDistApproval = 0)
        {
            try
            {
                List<string> sbnch = new List<string>();
                if (!string.IsNullOrWhiteSpace(RequisitionIdsInCommaSeparated.Trim()))
                {
                    sbnch = RequisitionIdsInCommaSeparated.Trim().Split(',').ToList();
                    foreach (var s in sbnch)
                    {
                        SqlCommand cmd = new SqlCommand("SP_Requisition_View_New_ForApproval");
                        cmd.CommandType = CommandType.StoredProcedure;

                        if(objRequisition.ISAPPROVED_ADMIN > 0) // admin section
                        {
                            cmd.Parameters.AddWithValue("@InStaus", objRequisition.ISAPPROVED_ADMIN);
                            cmd.Parameters.AddWithValue("@Opmode", 5);
                        }
                        else // district & director section
                        {
                            cmd.Parameters.AddWithValue("@InStaus", IsForDistApproval <= 0 ? (objRequisition.ISAPPROVED > default(int) ? objRequisition.ISAPPROVED : default(int)) : (objRequisition.ISAPPROVED_DIST > default(int) ? objRequisition.ISAPPROVED_DIST : default(int)));
                            cmd.Parameters.AddWithValue("@Opmode", IsForDistApproval <= 0 ? 2 : 4);
                            
                        }
                        cmd.Parameters.AddWithValue("@InUserId", objRequisition.UserId);
                        
                        cmd.Parameters.AddWithValue("@ReqID", Convert.ToInt32(s.Trim()));
                        
                        objDbUlility.ExNonQuery(cmd);
                    }
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionBySchoolId(Int64 SchoolId, string startDate, string endDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetRequisitionBySchoolId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetRequisitionViewDataByReqId(string ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_Requisition_View_ByReqId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable ChkDuplicateReqWhileUpdate(Int64 CircleID, Int64 SchoolID, Int64 CategoryID, Int64 LanguageID, string ReqId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_Chk_DuplicateReq_While_Update"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@LanguageID", LanguageID);
                    cmd.Parameters.AddWithValue("@ReqId", ReqId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable ChkDuplicateReqWhileInsert(Int64 CircleID, Int64 SchoolID, Int64 CategoryID, Int64 LanguageID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_Chk_DuplicateReq_While_Insert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@LanguageID", LanguageID);
                     ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public static string GenerateToXml(object obj)
        {
            StringWriter strWriter = new StringWriter();
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add("", "");
            XmlTextWriter xmlWriter = new XmlTextWriter(strWriter);
            xmlWriter.Formatting = Formatting.None;
            xmlWriter.WriteRaw("");
            XmlSerializer xSer = new XmlSerializer(obj.GetType());
            xSer.Serialize(xmlWriter, obj, xns);
            return strWriter.ToString();

        }
        #endregion

        #region SchProbChallan
        public bool InsertInSchProvisionalChallan(SchProvisionalChallan objSchProvisionalChallan, out string reqGenCode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertInSchProvisionalChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InChallanNo", objSchProvisionalChallan.ChallanNo);
                cmd.Parameters.AddWithValue("@InCircleId", objSchProvisionalChallan.CircleId);
                cmd.Parameters.AddWithValue("@InLanguageID", objSchProvisionalChallan.LanguageID);
                cmd.Parameters.AddWithValue("@InStaus", objSchProvisionalChallan.SaveStatus);
                cmd.Parameters.AddWithValue("@InRequisitionIds", objSchProvisionalChallan.InRequisitionIds);
                cmd.Parameters.AddWithValue("@InBookCodes", objSchProvisionalChallan.InBookCodes);
                cmd.Parameters.AddWithValue("@InAcademicYearID", objSchProvisionalChallan.AcademicYearID);
                cmd.Parameters.AddWithValue("@InUserId", objSchProvisionalChallan.UserId);
                cmd.Parameters.Add("InTrx_Sch_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchProvisionalChallanView(DateTime StartDate, DateTime EndDate, Int16 AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchProvisionalChallanView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchProbChallanByChallanId(Int64 InChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchProbChallanByChallanId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool SchRequisitionProbChallanConfirm(SchProvisionalChallan objSchProvisionalChallan, string InChallanIds)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Sch_Prob_Challan_Confirm");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InChallanIds", InChallanIds);
                cmd.Parameters.AddWithValue("@InStaus", objSchProvisionalChallan.SaveStatus);
                cmd.Parameters.AddWithValue("@InUserId", objSchProvisionalChallan.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchProvisionalChallanConfirmedView(DateTime StartDate, DateTime EndDate, Int16 AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_SchProvisionalChallanConfirmedView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    //cmd.Parameters.AddWithValue("@In_CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateSchChallan(SchProvisionalChallan objSchProvisionalChallan)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateSchChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InChallanId", objSchProvisionalChallan.ChallanId);
                cmd.Parameters.AddWithValue("@InAllotmentId", objSchProvisionalChallan.BinderAllotMentId);
                cmd.Parameters.AddWithValue("@InChallanQty", objSchProvisionalChallan.ChallanQty);
                cmd.Parameters.AddWithValue("@InUserId", objSchProvisionalChallan.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable IsValidBinderAllotmentBookDtl(Int32 InChallanId, Int32 InBinderAllotmentId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_IsValidBinderAllotmentBookDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    cmd.Parameters.AddWithValue("@InBinderAllotmentId", InBinderAllotmentId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetProbChallanBookDetailsById(Int32 InChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetProbChallanBookDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBookAllotedQtyByChallaId(Int32 InChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_BookAllotedQtyByChallaId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InChallanId", InChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region Stock
        public DataTable GetReqStokDetails(Int64 CategoryID, Int64 LanguageID, Int64 CircleID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetReqStokDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@LanguageID", LanguageID);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetReqStokDetailsWithTrnf(Int64 CategoryID, Int64 LanguageID, Int64 CircleID, int destCircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetReqStokDetailsWithTrnf"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleID", CircleID);
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@LanguageID", LanguageID);
                    cmd.Parameters.AddWithValue("@DestCircleID", destCircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBookWiseReqStokDetails(Int64 CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBookWiseReqStokDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetDirectorateCircleWiseSchoolReport(Int64 CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_DirectorateCircleWiseSchoolReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable CircleWiseRequisitionStock()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleWiseRequisitionStock"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable CircleWiseRequisitionStockByDistrictID(string districtId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_CircleWiseRequisitionStockByDistrictID"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_DistrictId", districtId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public DataSet GetMisReport(Int64 CircleId, string ddlType, string ddlClassCategory, string ddlLanguageName, string ddlBookName, string txtStartDate, string txtEndDate)
        {
           

            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_MisReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CircleId", CircleId);
                    // cmd.Parameters.AddWithValue("@ddlType", ddlType);
                    if (ddlBookName.Contains("ALL"))
                        cmd.Parameters.AddWithValue("@ddlBookName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ddlBookName", ddlBookName);

                    if (ddlLanguageName.Contains("ALL"))
                        cmd.Parameters.AddWithValue("@ddlLanguageName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ddlLanguageName", ddlLanguageName);

                    if (ddlClassCategory.Contains("ALL"))
                        cmd.Parameters.AddWithValue("@ddlClassCategory", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ddlClassCategory", ddlClassCategory);

                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        //public DataTable GetDistrictCircleWiseMisReport1(Int64 DistrictId)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("Sp_DistrictCircleWiseMisReport");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@DistrictId", DistrictId);
        //        DataTable ObjDataTable = objDbUlility.GetDataTable(cmd);
        //        return ObjDataTable;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //    finally { }
        //}

        public DataTable GetDirectorMisReport()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_DirectorMisReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataSet GetDistrictWiseMisReport()
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_DirectorMisReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataSet GetDistrictCircleWiseMisReport(Int64 DistrictId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_DistrictCircleWiseMisReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataSet GetCircleWiseChallanDelivaryReport(Int64 DistrictId, string usertype)
        {
            // modified on 8.12.18 Pomeli

            try
            {
                DataSet ObjDataTable;
                if(usertype=="7") // for SPL user. spl user can view effect only after admin approval
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_CircleWiseChallanDelivaryReportSPLUser"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DistrictId", DistrictId);
                        ObjDataTable = objDbUlility.GetDataSet(cmd);
                    }
                    return ObjDataTable;
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_CircleWiseChallanDelivaryReport"))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DistrictId", DistrictId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                    return ObjDataTable;
                }
                
                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetDistrictWiseChallanDelivaryReport(string usertype)
        {
            // modified on 8.12.18 Pomeli

            
                 try
                {
                    DataSet ObjDataTable;
                     if (usertype == "7") // for SPL user. spl user can view effect only after admin approval
                     {
                         using (SqlCommand cmd = new SqlCommand("Sp_DistrictWiseChallanDelivaryReportSPLUser"))
                         {
                             cmd.CommandType = CommandType.StoredProcedure;
                             ObjDataTable = objDbUlility.GetDataSet(cmd);
                         }
                        return ObjDataTable;
                     }

                     else
                     {

                         using (SqlCommand cmd = new SqlCommand("Sp_DistrictWiseChallanDelivaryReport"))
                         {
                             cmd.CommandType = CommandType.StoredProcedure;
                             ObjDataTable = objDbUlility.GetDataSet(cmd);
                         }
                         return ObjDataTable;

                     }
                }
            
            

            
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertInReqStokDetails(StockUpdate objcust, Int64 CircleId)
        {
            try
            {
                // var removeList = new List<int>() { 0 };
                // objcust.reqStockCollection.RemoveAll(r => removeList.Any(a => a == r.StockUpdateQuantity));
                SqlCommand cmd = new SqlCommand("Sp_InsertInReqStokDetails");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleId", CircleId);
                cmd.Parameters.AddWithValue("@categoryID", objcust.CategoryID);
                cmd.Parameters.AddWithValue("@languageID", objcust.LanguageID);
                cmd.Parameters.AddWithValue("@UserId", objcust.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objcust.reqStockCollection);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<StockTrxDtl>(objcust.reqStockCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetDistrictCircleWiseRequisitionStock(string DistID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_DistrictCircleWiseRequisitionStock"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DistID", DistID);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertUpdateCircleStockDamage(string xData)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CircleStockDamageInsertUpdateBatch");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("xData", SqlDbType.NVarChar);
                cmd.Parameters["xData"].Value = xData;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Invoice Cum Challan
        public DataTable GetSPLoginDtl(string sp_user_name, string sp_password)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSPLoginDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", sp_user_name);
                    cmd.Parameters.AddWithValue("@UsrPassword", sp_password);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetSpChallanDtl(string InvoiceCumChallanNo)
        {
            try
            {
                DataSet ObjDataSet;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSpChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_mode", 1);
                    cmd.Parameters.AddWithValue("@in_DistrictId", null);
                    cmd.Parameters.AddWithValue("@in_CircleId", null);
                    cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", InvoiceCumChallanNo);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_NUMBER", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_DATE", null);
                    cmd.Parameters.AddWithValue("@in_iccSCHOOL_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccBOOK_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccREQUISITION_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccDELIVERED_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccBALANCE_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccTRANSPORTER_NAME", null);
                    cmd.Parameters.AddWithValue("@in_iccCONSIGNEE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccVEHICLE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccDistrictId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircleId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_Address", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_PinCode", null);
                    ObjDataSet = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataSet;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSpChallanDistrictCircleDtl(string DistrictId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSpChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_mode", 2);
                    cmd.Parameters.AddWithValue("@in_DistrictId", DistrictId);
                    cmd.Parameters.AddWithValue("@in_CircleId", null);
                    cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_NUMBER", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_DATE", null);
                    cmd.Parameters.AddWithValue("@in_iccSCHOOL_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccBOOK_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccREQUISITION_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccDELIVERED_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccBALANCE_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccTRANSPORTER_NAME", null);
                    cmd.Parameters.AddWithValue("@in_iccCONSIGNEE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccVEHICLE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccDistrictId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircleId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_Address", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_PinCode", null);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSpChallanCircleAddPinDtl(string DistrictId, string CircleId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSpChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_mode", 3);
                    cmd.Parameters.AddWithValue("@in_DistrictId", DistrictId);
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_NUMBER", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_DATE", null);
                    cmd.Parameters.AddWithValue("@in_iccSCHOOL_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccBOOK_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccREQUISITION_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccDELIVERED_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccBALANCE_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccTRANSPORTER_NAME", null);
                    cmd.Parameters.AddWithValue("@in_iccCONSIGNEE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccVEHICLE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccDistrictId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircleId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_Address", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_PinCode", null);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetSpChallanReqDtl()
        {
            try
            {
                DataSet ObjDataSet;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSpChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_mode", 5);
                    cmd.Parameters.AddWithValue("@in_DistrictId", null);
                    cmd.Parameters.AddWithValue("@in_CircleId", null);
                    cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_NUMBER", null);
                    cmd.Parameters.AddWithValue("@in_iccCHALLAN_DATE", null);
                    cmd.Parameters.AddWithValue("@in_iccSCHOOL_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccBOOK_ID", null);
                    cmd.Parameters.AddWithValue("@in_iccREQUISITION_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccDELIVERED_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccBALANCE_QTY", null);
                    cmd.Parameters.AddWithValue("@in_iccTRANSPORTER_NAME", null);
                    cmd.Parameters.AddWithValue("@in_iccCONSIGNEE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccVEHICLE_NO", null);
                    cmd.Parameters.AddWithValue("@in_iccDistrictId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircleId", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_Address", null);
                    cmd.Parameters.AddWithValue("@in_iccCircle_PinCode", null);
                    ObjDataSet = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataSet;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanListDtl(string CircleId, string SchoolCategoryId, string LanguageId, string ChallanID, int AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanListDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", SchoolCategoryId);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", LanguageId);
                    cmd.Parameters.AddWithValue("@ChallanID", ChallanID);
                    cmd.Parameters.AddWithValue("@AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanListDtlForBarcode(string CircleId, string SchoolCategoryId, string LanguageId, string ChallanID, int AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanListDtlForBarcode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", SchoolCategoryId);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", LanguageId);
                    cmd.Parameters.AddWithValue("@ChallanID", ChallanID);
                    cmd.Parameters.AddWithValue("@AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanListDtlForRevert(string CircleId, string SchoolCategoryId, string LanguageId, string ChallanID, int AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanListDtlForRevert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_CircleId", CircleId);
                    cmd.Parameters.AddWithValue("@in_iccCATEGORY_ID", SchoolCategoryId);
                    cmd.Parameters.AddWithValue("@in_iccLANGUAGE_ID", LanguageId);
                    cmd.Parameters.AddWithValue("@ChallanID", ChallanID);
                    cmd.Parameters.AddWithValue("@AccadYear", AccadYear);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetLiveBinderBookStatusOnScan(string binderAllotCode, int categoryId, int AccadYear)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetLiveBinderBookStatusOnScan"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@binderAllotCode", binderAllotCode);
                    cmd.Parameters.AddWithValue("@categoryID", categoryId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetProvisionalChallanDetails(string startDate, string endDate, string CircleID, string DistrictID, int AccadYearId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetProvisionalChallanView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@AccadYearId", AccadYearId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanDetailsByIdSimplified(int ChallanId)
        {
            try
            {
                 DataTable ObjDataTable;
                 using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDetailsByIdSimple"))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@ChallanId", ChallanId);
                     ObjDataTable = objDbUlility.GetDataTable(cmd);
                 }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetProvisionalChallanViewModified(string startDate, string endDate, string CircleID, string DistrictID, int AccadYearId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetProvisionalChallanViewModified"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@AccadYearId", AccadYearId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetPendingChallanDtlBasedOnDayDiff(int DayDiff, string CircleID, string DistrictID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetPendingChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@DiffDays", DayDiff);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetPendingChallanDtlBasedOnDayDiffDDL(int DayDiff, string CircleID, string DistrictID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetPendingChallanDtlDDL"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@DiffDays", DayDiff);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanDtlModified(DateTime startDate, DateTime endDate, string CircleID, string DistrictID, string challanNumber, int inStatus = 1, string strCondition = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(challanNumber))
                {
                    strCondition = " AND TC.CHALLAN_NUMBER LIKE '" + challanNumber + "%'";
                }
                else
                {
                    strCondition = "";
                }
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDtlModified"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inStatus", inStatus);
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@FromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    cmd.Parameters.AddWithValue("@strCondition", strCondition);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetbinderBookQtyReport(DateTime startDate, DateTime endDate, int BinderId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderBookQtyReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BinderId", BinderId);
                    cmd.Parameters.AddWithValue("@FromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanDtlModifiedMinimal(DateTime startDate, DateTime endDate, string CircleID, string DistrictID, int inStatus = 1)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDtlModifiedMinimal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inStatus", inStatus);
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@FromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanDtl(DateTime startDate, DateTime endDate, string CircleID, string DistrictID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@inDistrictId", DistrictID);
                    cmd.Parameters.AddWithValue("@FromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetChallanDtlByCircleId(string startDate, string endDate, string CircleID, string PendingOnly)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDtlByCircleId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inCircleId", CircleID);
                    cmd.Parameters.AddWithValue("@PendingOnly", PendingOnly);
                    //cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate + " 00:00:00.000"));
                    //cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate + " 23:59:59.999"));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable UpdateCircleChallanReceived(string ChannalID, string UserId, string ReceiveDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_UpdateCircleChallanReceived"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChannalID", ChannalID);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@ReceiveDate", Convert.ToDateTime(ReceiveDate));

                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateCircleChallanCommentById(long ChannalID, string comment  , string createdby)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateCircleChallanComment");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChannalID", ChannalID);
                cmd.Parameters.AddWithValue("@Comment", comment);
                cmd.Parameters.AddWithValue("@CREATED_By", createdby);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        // 3-12-18 Update/add challan comment in received challan list (circle user)

        public bool UpdateCircleChallanCommentById_SeparateTable(int ChannalID, string comment, string createdby)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_RcvChallanCommentUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanId", ChannalID);
                cmd.Parameters.AddWithValue("@ChallanComment", comment);
                cmd.Parameters.AddWithValue("@CREATED_By", createdby);
                cmd.Parameters.AddWithValue("@Opmode", 1);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        // 3-12-18 Get challan comment for a challan entry by challan id (circle user)

        public DataTable GetCircleChallanCommentById_SeparateTable(int ChannalID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("usp_RcvChallanCommentUpdate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", ChannalID);
                    cmd.Parameters.AddWithValue("@Opmode", default(int));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        //mobile
        public bool MobileCircleChallanReceived(MobileReceipt mobileReceipt)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MobileRcpt");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanBarcode", mobileReceipt.ChallanBarcode);
                cmd.Parameters.AddWithValue("@PhoneNo", mobileReceipt.PhoneNo);
                cmd.Parameters.AddWithValue("@ReceiverCode", mobileReceipt.ReceiverCode);
                cmd.Parameters.AddWithValue("@ReceiverPic", mobileReceipt.ReceiverPic);
                cmd.Parameters.AddWithValue("@SendersIP", mobileReceipt.SendersIP);
                cmd.Parameters.AddWithValue("@Place", mobileReceipt.Place);
                cmd.Parameters.AddWithValue("@UserID", mobileReceipt.UserID);
                cmd.Parameters.AddWithValue("@DeviceUUID", mobileReceipt.DeviceUUID);
                cmd.Parameters.AddWithValue("@Opmode", 1);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSMSCodeByChallan(string challan)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_MobileRcpt"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanBarcode", challan);
                    cmd.Parameters.AddWithValue("@Opmode", 0);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetIDByChallanBarcode(string challan)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("SP_MobileRcpt"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanBarcode", challan);
                    cmd.Parameters.AddWithValue("@Opmode", 2);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                    return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetChallanDetailsById(Int64 ChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", ChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetProvisionalChallanDetailsById(Int64 ChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetProvisionalChallanDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", ChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }



        public DataTable GetChallanPrintDetailsById(Int64 ChallanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanPrintDetailsById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", ChallanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInChallan(InvoiceCumChallan objInvCumChal, out string reqGenCode)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objInvCumChal.InvoiceCumChallanCollection.RemoveAll(r => removeList.Any(a => a == r.QtyShipped));

                SqlCommand cmd = new SqlCommand("Sp_InsertInChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", objInvCumChal.InvoiceCumChallanNo);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objInvCumChal.InvoiceCumChallanCollection);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<InvoiceCumChallanList>(objInvCumChal.InvoiceCumChallanCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool InsertInProvisionalChallan(InvoiceCumChallan objInvCumChal, out string reqGenCode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertInProvisionalChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", objInvCumChal.InvoiceCumChallanNo);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.AddWithValue("@InAcadYearId", objInvCumChal.AcadYearId);
                cmd.Parameters.AddWithValue("@ManualChallanNo", objInvCumChal.ManualChallanNo);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInChallan(InvoiceCumChallan objInvCumChal)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_UpdateInChallan";
                cmd.Parameters.AddWithValue("@ChallanId", objInvCumChal.ChallanId);
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", objInvCumChal.InvoiceCumChallanNo);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objInvCumChal.InvoiceCumChallanCollection);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<InvoiceCumChallanList>(objInvCumChal.InvoiceCumChallanCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool InsertInChallanNew(InvoiceCumChallan objInvCumChal, string barcodes, string duplicatebarcodes, out string reqGenCode)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objInvCumChal.InvoiceCumChallanCollection.RemoveAll(r => removeList.Any(a => a == r.QtyShipped));

                SqlCommand cmd = new SqlCommand("Sp_InsertInChallanNew");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", objInvCumChal.InvoiceCumChallanNo);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.AddWithValue("@Barcodes", barcodes);
                cmd.Parameters.AddWithValue("@duplicatebarcodes", duplicatebarcodes);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objInvCumChal.InvoiceCumChallanCollection);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<InvoiceCumChallanList>(objInvCumChal.InvoiceCumChallanCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool UpdateInChallanNew(InvoiceCumChallan objInvCumChal, string barcodes, string duplicatebarcodes, bool isDraft = false)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_UpdateInChallanAsDraft";
                if (isDraft)
                {

                    cmd.Parameters.AddWithValue("@Status", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Status", 2);
                }

                cmd.Parameters.AddWithValue("@Barcodes", barcodes);
                cmd.Parameters.AddWithValue("@duplicatebarcodes", duplicatebarcodes);
                cmd.Parameters.AddWithValue("@ChallanId", objInvCumChal.ChallanId);
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_InvoiceCumChallanNo", objInvCumChal.InvoiceCumChallanNo);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objInvCumChal.InvoiceCumChallanCollection);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<InvoiceCumChallanList>(objInvCumChal.InvoiceCumChallanCollection)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateFinalChallanHeaderNew(InvoiceCumChallan objInvCumChal)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateFinalChallanHeaderNew");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InID", objInvCumChal.ChallanId);
                cmd.Parameters.AddWithValue("@InVehicleNo", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@InConsigneeNo", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@InTransporterId", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@InUserId", objInvCumChal.UserId);
                cmd.Parameters.AddWithValue("@ManualChallanNo", objInvCumChal.ManualChallanNo);//String.IsNullOrEmpty(objInvCumChal.ManualChallanNo) ? DBNull.Value : objInvCumChal.ManualChallanNo);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInProvisionalChallan(InvoiceCumChallan objInvCumChal)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateInProvisionalChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanId", objInvCumChal.ChallanId);
                cmd.Parameters.AddWithValue("@in_CircleId", objInvCumChal.CircleId);
                cmd.Parameters.AddWithValue("@in_CHALLAN_DATE", Convert.ToDateTime(objInvCumChal.InvoiceCumChallanDate));
                cmd.Parameters.AddWithValue("@in_CATEGORY_ID", objInvCumChal.CategoryId);
                cmd.Parameters.AddWithValue("@in_LANGUAGE_ID", objInvCumChal.LanguageId);
                cmd.Parameters.AddWithValue("@in_TRANSPORTER_ID", objInvCumChal.TransporterID);
                cmd.Parameters.AddWithValue("@in_CONSIGNEE_NO", objInvCumChal.CONSIGNEE_NO);
                cmd.Parameters.AddWithValue("@in_VEHICLE_NO", objInvCumChal.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@UserId", objInvCumChal.UserId);
                cmd.Parameters.AddWithValue("@ManualChallanNo", objInvCumChal.ManualChallanNo);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetUnBilledChallanDtlByDistrict(Int64 districtId, string startDate, string endDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetUnBilledChallanDtlByDistrict"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@districtId", districtId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }


        public DataTable GetlotfromchallanID(string challanID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetlotfromchallanID"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@challanID", challanID);

                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool ConfirmProvisionalChallan(InvoiceCumChallan objInvoiceCumChallan, string RequisitionIdsInCommaSeparated)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_ProvisionalChallanConfirm");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequisitionIds", RequisitionIdsInCommaSeparated);
                cmd.Parameters.AddWithValue("@InStaus", objInvoiceCumChallan.Status);
                cmd.Parameters.AddWithValue("@InUserId", objInvoiceCumChallan.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateBinderDtlOnScan(int challanId, string binderAllotCode, string userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateBinderDtlOnScan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                cmd.Parameters.AddWithValue("@binderAllotCode", binderAllotCode);
                cmd.Parameters.AddWithValue("@userId", userId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateBinderDtlOnScanBarcode(int challanId, string binderAllotCode, string userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateBinderDtlOnScanBarcode");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                cmd.Parameters.AddWithValue("@binderAllotCode", binderAllotCode);
                cmd.Parameters.AddWithValue("@userId", userId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateBinderDtlOnScanUndo(int challanId, string fullstr, string userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateBinderDtlOnScanUndo");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                cmd.Parameters.AddWithValue("@fullstr", fullstr);
                cmd.Parameters.AddWithValue("@userId", userId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateBinderDtlOnScanUndoSingle(int challanId, string fullstr, string userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateBinderDtlOnScanUndoSingle");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                cmd.Parameters.AddWithValue("@barcodeDtlID", Convert.ToInt32(fullstr));
                cmd.Parameters.AddWithValue("@userId", userId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateBinderDtlOnScanUndoSingleNew(int challanId, string fullstr, string userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateBinderDtlOnScanUndoSingleNew");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                cmd.Parameters.AddWithValue("@barcode", fullstr);
                cmd.Parameters.AddWithValue("@userId", userId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateChallanRevertById(string fullstr, string userId)
        {
            try
            {
                var invoiceIDsToUpdate = fullstr.Split(',');
                foreach (var invoiceId in invoiceIDsToUpdate)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("usp_FullChallanCancel");
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@ChallanID", Convert.ToInt32(invoiceId));
                        objDbUlility.ExNonQuery(cmd);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool PartialChallanRevertUpdate(int challanId, string userId, IEnumerable<RevisedQtyMap> lst)
        {
            bool result = default(bool);
            try
            {
                if (lst != null && lst.Count() > default(int))
                {
                    foreach (var item in lst)
                    {
                        SqlCommand cmd = new SqlCommand("usp_PartialChallanRevertUpdate");
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@challanId", challanId);
                        cmd.Parameters.AddWithValue("@book_code", item.Book_Code);
                        cmd.Parameters.AddWithValue("@revised_qty", item.RevisedQty);
                        cmd.Parameters.AddWithValue("@cancelled_qty", item.CancelledQty);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        objDbUlility.ExNonQuery(cmd);
                    }
                    result = true;
                }
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateChallanConfirmById(string fullstr, string userId)
        {
            try
            {
                var invoiceIDsToUpdate = fullstr.Split(',');
                foreach (var invoiceId in invoiceIDsToUpdate)
                {

                    SqlCommand cmd = new SqlCommand("Sp_UpdateChallanConfirmById");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fullstr", Convert.ToInt16(invoiceId));
                    cmd.Parameters.AddWithValue("@userId", userId);
                    objDbUlility.ExNonQuery(cmd);
                }

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetConfirmedChallanInfoById(string fullstr)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetConfirmedChallanInfoById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fullstr", fullstr);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotDetailByChallanId(int challanId, string userId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAllotDetailByChallanId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@challanId", challanId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAlotDtlByAllotCode(string binderAllotCode)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderAlotDtlByAllotCode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@binderAllotCode", binderAllotCode);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        #endregion

        #region School Challan
        public bool InsertInSchoolChallan(SchoolChallan objSchoolChallan, out string reqGenCode)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objSchoolChallan.trxSchoolChallanBookReqDtl.RemoveAll(r => removeList.Any(a => a == r.QuantityForShipping));

                SqlCommand cmd = new SqlCommand("Sp_InsertInSchoolChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_ReqId", objSchoolChallan.RequisitionId);
                cmd.Parameters.AddWithValue("@in_ChallanNo", objSchoolChallan.SchoolChallanCode);
                cmd.Parameters.AddWithValue("@in_ChallanDate", Convert.ToDateTime(objSchoolChallan.SchoolChallanDate));
                cmd.Parameters.AddWithValue("@UserId", objSchoolChallan.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objSchoolChallan.trxSchoolChallanBookReqDtl);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<SchoolChallanBookReqDtl>(objSchoolChallan.trxSchoolChallanBookReqDtl)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateInSchoolChallan(SchoolChallan objSchoolChallan)
        {
            try
            {
                var removeList = new List<int>() { 0 };
                objSchoolChallan.trxSchoolChallanBookReqDtl.RemoveAll(r => removeList.Any(a => a == r.QuantityForShipping));

                SqlCommand cmd = new SqlCommand("Sp_UpdateInSchoolChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_ReqId", objSchoolChallan.RequisitionId);
                cmd.Parameters.AddWithValue("@in_ChallanId", objSchoolChallan.SchoolChallanUniqueId);
                cmd.Parameters.AddWithValue("@in_ChallanDate", Convert.ToDateTime(objSchoolChallan.SchoolChallanDate));
                cmd.Parameters.AddWithValue("@UserId", objSchoolChallan.UserId);
                cmd.Parameters.Add("trx_requisition_dtl_xml", SqlDbType.NVarChar);
                //cmd.Parameters["trx_requisition_dtl_xml"].Value = GenerateToXml(objSchoolChallan.trxSchoolChallanBookReqDtl);
                cmd.Parameters["trx_requisition_dtl_xml"].Value = Utility.CreateXmlTraditional(Utility.ToDataTable<SchoolChallanBookReqDtl>(objSchoolChallan.trxSchoolChallanBookReqDtl)).InnerXml;
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetSchoolChallanViewBySchooldId(Int64 SchoolId, Int64 CircleId, string startDate, string endDate)
        {
            try
            {
                DataTable ObjDataTable;

                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolChallanViewBySchooldId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                    cmd.Parameters.AddWithValue("@CircleId", CircleId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetRequisitionDtlByReqId(Int64 ReqId, Int64 challanId)
        {
            try
            {
                DataTable ObjDataTable;
                using ( SqlCommand cmd = new SqlCommand("Sp_GetRequisitionDtlByReqId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_ReqId", ReqId);
                    cmd.Parameters.AddWithValue("@in_challanId", challanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetSchoolChallanPrintDtl(Int64 challanId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetSchoolChallanPrintDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inChallanId", challanId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Save System Error Log
        public void SaveSystemErrorLog(Exception e, string IPAddress = "")
        {

            StringBuilder sbErrorText = new StringBuilder();
            string FileName = DateTime.Now.ToString("dd-MMM-yyyy");

            string ErrorLogPath = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorLogPath"].ToString();


            sbErrorText.Append(Environment.NewLine);
            sbErrorText.Append(Environment.NewLine);
            sbErrorText.Append("======================================================================================" + Environment.NewLine);
            sbErrorText.Append("@  Error Date Time  : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + Environment.NewLine);
            sbErrorText.Append("@  Error At  : " + IPAddress + " " + Environment.NewLine);
            sbErrorText.Append("--------------------------------------------------------------------------------------" + Environment.NewLine);
            sbErrorText.Append("@  Message            : " + e.Message + Environment.NewLine);
            sbErrorText.Append("@  Inner Exception    : " + e.InnerException + Environment.NewLine);
            sbErrorText.Append("@  Source             : " + e.Source + Environment.NewLine);
            sbErrorText.Append("@  Stack Trace        : " + e.StackTrace + Environment.NewLine);
            sbErrorText.Append("@  TargetSite         : " + e.Data + Environment.NewLine);
            sbErrorText.Append("--------------------------------------------------------------------------------------" + Environment.NewLine);
            string Path = ErrorLogPath + @"\" + FileName + ".txt";
            string TemplatePath = ErrorLogPath + @"\Errors.txt";
            if (!File.Exists(Path))
            {
                try
                {
                    File.Copy(TemplatePath, Path);
                }
                catch (Exception ex) { }
            }

            if (File.Exists(Path))
            {
                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(Path, true);
                    file.WriteLine(sbErrorText);
                    file.Close();
                }
                catch (Exception ex) { }

            }


        }
        #endregion

        #region Invoice
        public bool InsertInInvoice(Invoice objInvoice, out string reqGenCode, out string retInvoiceId)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_InsertInInvoice");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_InvoiceNo", objInvoice.InvoiceNo);
                cmd.Parameters.AddWithValue("@in_ManualInvoiceNo", objInvoice.ManualInvoiceNo);
                cmd.Parameters.AddWithValue("@in_InvoiceDate", Convert.ToDateTime(objInvoice.InvoiceDate));
                cmd.Parameters.AddWithValue("@in_ChallanId", objInvoice.ChallanId);
                cmd.Parameters.AddWithValue("@in_CategoryId", objInvoice.CategoryId);
                cmd.Parameters.AddWithValue("@UserId", objInvoice.UserId);
                cmd.Parameters.Add("@reqGenCode", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@retInvoiceId", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                objDbUlility.ExNonQuery(cmd);
                reqGenCode = cmd.Parameters["@reqGenCode"].Value.ToString();
                retInvoiceId = cmd.Parameters["@retInvoiceId"].Value.ToString();

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool AddChallanInInvoice(Invoice objInvoice)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_AddChallanInInvoice");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_InvoiceId", objInvoice.InvoiceId);
                cmd.Parameters.AddWithValue("@in_ChallanId", objInvoice.ChallanId);
                cmd.Parameters.AddWithValue("@UserId", objInvoice.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataSet GetChallanCodeOfaCategoryDtl(string ChallanNo, Int64 CategoryId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetChallanCodeOfaCategoryDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_ChallanNo", ChallanNo);
                    cmd.Parameters.AddWithValue("@in_CategoryId", CategoryId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetInvoiceChallanDetails(Int64 InvoiceId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetInvoiceChallanDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@in_InvoiceId", InvoiceId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetInvoiceAnnexureI(Int64 InvoiceId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_InvoiceAnnexureI"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inInvoiceId", InvoiceId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetInvoiceViewDtl(string startDate, string endDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetInvoiceViewDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetInvoiceDetailById(Int64 InvoiceId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetInvoiceDetailById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inInvoiceId", InvoiceId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool DeleteChallanFromInvoice(string in_InvoiceDtlId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_DeleteChallanFromInvoice");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_InvoiceDtlId", in_InvoiceDtlId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public bool UpdateInvoiceDtl(Invoice objInvoice)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UpdateInvoiceDtl");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_ManualInvoiceNo", objInvoice.ManualInvoiceNo);
                cmd.Parameters.AddWithValue("@in_InvoiceDate", Convert.ToDateTime(objInvoice.InvoiceDate));
                cmd.Parameters.AddWithValue("@in_Save_Status", objInvoice.SaveStatus);
                cmd.Parameters.AddWithValue("@in_InvoiceId", objInvoice.InvoiceId);
                cmd.Parameters.AddWithValue("@UserId", objInvoice.UserId);
                objDbUlility.ExNonQuery(cmd);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetInvoiceAnnexureII(Int64 inInvoiceId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_InvoiceAnnexureII"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inInvoiceId", inInvoiceId);
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Login
        public DataTable GetBDMSLoginDtl(string userName, string Password, int InAcadYearId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBDMSLoginDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    cmd.Parameters.AddWithValue("@UsrPassword", SecurityController.Encrypt(Password));
                    cmd.Parameters.AddWithValue("@InAcadYearId", InAcadYearId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBDMSLoginDtlMobile(string userName, string Password)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBDMSLoginDtlMobile"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    cmd.Parameters.AddWithValue("@UsrPassword", Password);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        // For Password Reset
        public DataTable GetBDMSUserDtlByUserName(string userName)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBDMSUserDtlByUsername"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public string CreatePasswordResetRequest(string userName, string mobile)
        {
            try
            {
                int result = default(int);
                string otp = new Random().Next(10000, 99999).ToString();
                using (SqlCommand cmd = new SqlCommand("Sp_CreatePasswordResetRequest"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@OTP", otp);
                    result = objDbUlility.ExNonQuery(cmd);
                    //var result = cmd.ExecuteNonQuery();
                }

                if (result > 0)
                {
                    return otp;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
            return null;
        }

        public bool ValidatePasswordResetRequest(string userName, string otp)
        {
            try
            {
                bool result = default(bool);
                using (SqlCommand cmd = new SqlCommand("Sp_GetPasswordResetRequest"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    DataTable ObjDataTable = objDbUlility.GetDataTable(cmd);
                    if (ObjDataTable.Rows.Count == 1)
                    {
                        if (ObjDataTable.Rows[0]["otp"].ToString() == otp && Convert.ToDateTime(ObjDataTable.Rows[0]["expires"]) >= DateTime.Now)
                        {
                            result = true;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateUserPassword(string userName, string password)
        {
            try
            {
                bool x = default(bool);
                using (SqlCommand cmd = new SqlCommand("Sp_UpdateUserPassword"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userName);
                    cmd.Parameters.AddWithValue("@Password", SecurityController.Encrypt(password));
                    var result = objDbUlility.ExNonQuery(cmd);
                    if (result > 0)
                    {
                        x = true;
                    }
                }
                return x;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool UpdateUserPasswordBulkEncryptDecrypt(IEnumerable<UserObject> lst)
        {
            try
            {
                foreach (var item in lst)
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_AllUserDetailsPlain"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", item.ID);
                            cmd.Parameters.AddWithValue("@REF_ID", item.REF_ID);
                            cmd.Parameters.AddWithValue("@USER_ID", item.USER_ID);
                            cmd.Parameters.AddWithValue("@PASSWORD", item.PASSWORD);
                            cmd.Parameters.AddWithValue("@IsPasswordEncrypted", item.IsPasswordEncrypted);
                            cmd.Parameters.AddWithValue("@Opmode", 1);
                            var result = objDbUlility.ExNonQuery(cmd);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
            return true;
        }

        public DataTable GetAllUsersForPasswordEncryptDecrypt()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("usp_AllUserDetailsPlain"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Opmode", default(int));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Challan
        public DataTable GetBinderDtlListByChallanIdOnly(string ChallanID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_GetBinderDtlListByChallanIDOnly"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanID", ChallanID);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataSet GetChallanBinderDtl(string startDate, string endDate, string InRemId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_ChallanBinderDtl"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InBookCode", InRemId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetBinderAllotmentQtyViewNew(DateTime StartDate, DateTime EndDate, Int16 AccadYear, int RowStartIndex, int RowEndIndex, string allotmentcode, string strSortFields = " A.BINDER_ALLOT_CODE DESC ", string strCondition = "")
        {
            try
            {
                DataTable ObjDataTable;
                if (!string.IsNullOrEmpty(allotmentcode))
                {
                    strCondition = " AND A.BINDER_ALLOT_CODE LIKE '" + allotmentcode + "%'";
                }
                else
                {
                     strCondition = "";
                }

                using (SqlCommand cmd = new SqlCommand("usp_GetBinderAllotmentQtyViewNew"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@In_FromDate", Convert.ToDateTime(StartDate));
                    cmd.Parameters.AddWithValue("@In_ToDate", Convert.ToDateTime(EndDate));
                    cmd.Parameters.AddWithValue("@In_AccadYear", AccadYear);
                    cmd.Parameters.AddWithValue("@RowStartIndex", RowStartIndex);
                    cmd.Parameters.AddWithValue("@RowEndIndex", RowEndIndex);
                    cmd.Parameters.AddWithValue("@strSortFields", strSortFields);
                    cmd.Parameters.AddWithValue("@strCondition", strCondition);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public bool CancelChallan(int challanId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CancelChallan");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@challanId", challanId);
                var result = objDbUlility.ExNonQuery(cmd);
                return result > 0;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region Report
        public DataSet GetBinderWiseBookQtyRpt(string startDate, string endDate, string InRemId)
        {
            try
            {
                DataSet ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("Sp_BinderWiseBookQtyRpt"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InRemId", InRemId);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(startDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(endDate));
                    ObjDataTable = objDbUlility.GetDataSet(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetBookSummaryRpt(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("uspGetSummaryStatusforBooks"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanDatefrom", startDate);
                    cmd.Parameters.AddWithValue("@ChallanDateto", endDate);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetKhataSummaryRpt(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("uspGetSummaryStatusforKhatas"))
                { 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ChallanDatefrom", startDate);
                cmd.Parameters.AddWithValue("@ChallanDateto", endDate);
                ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetDistBookSummaryRpt(DateTime startDate, DateTime endDate, int DistrictId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("uspGetSummaryStatusforBooksDistrictwise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanDatefrom", startDate);
                    cmd.Parameters.AddWithValue("@ChallanDateto", endDate);
                    cmd.Parameters.AddWithValue("@DistId", DistrictId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        public DataTable GetDistKhataSummaryRpt(DateTime startDate, DateTime endDate, int DistrictId)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("uspGetSummaryStatusforKhatasDistrictwise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanDatefrom", startDate);
                    cmd.Parameters.AddWithValue("@ChallanDateto", endDate);
                    cmd.Parameters.AddWithValue("@DistId", DistrictId);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        // modified 4-12-18
        public DataTable GetGDBNB()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("usp_GDBNB"))
                {
                cmd.CommandType = CommandType.StoredProcedure;
                ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion

        #region [Academic Year]
        public DataTable GetAllAcademicYear()
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("usp_AcademicYear"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Opmode", default(int));
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                }
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }

        public DataTable GetAcademicYearByID(int ID)
        {
            try
            {
                DataTable ObjDataTable;
                using (SqlCommand cmd = new SqlCommand("usp_AcademicYear"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Opmode", 1);
                    ObjDataTable = objDbUlility.GetDataTable(cmd);
                } 
                return ObjDataTable;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { }
        }
        #endregion [Academic Year]
    }
}