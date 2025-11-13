select distinct NoticeEmail  from  ViewDeptWiseComplainDailyReport where TenentID = 10 and UploadDate BETWEEN ' 2020-10-21 12:00:00 AM ' AND ' 2020-10-21 11:59:59 PM ' 



select ComplaintNumber as 'ComplaintNumber' , DeptName as 'DeptName' , MasterCODE as 'MasterCODE' , TickComplainType as 'complain' , TickDepartmentID as 'Department' , 
                         Patient_Name as Name , MRN as 'MRN' , aspcomment as 'aspcomment' , MyStatus as MyStatus , Patient_Name as Patient_Name, investigation as investigation , MRN as MRN , Contact as Contact ,NoticeEmail as 'NoticeEmail' , TickCatID as 'Category' , 
                        TickSubCatID as 'SubCategory' , ActivityPerform as 'Remarks', ReportedBy as 'ReportedBy' , UploadDate as 'UploadDate' 
                         from ViewDeptWiseComplainDailyReport where TenentID = 10 and UploadDate BETWEEN '2020-10-22 12:00:00 AM' AND '2020-10-22 11:59:59 PM' 
                         and NoticeEmail='dangijalpa@gmail.com,johar@writeme.com,yehia.khafaja@royalehayat.com'  order by TickDepartmentID , NoticeEmail