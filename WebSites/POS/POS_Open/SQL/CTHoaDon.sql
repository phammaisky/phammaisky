CREATE TABLE @CTHoaDon@(
	[MaHD] [nvarchar](50) NOT NULL,
	[MaHH] [nvarchar](20) NOT NULL,
	[STT] [int] NOT NULL,
	[NgayGioQuet] [datetime] NULL,
	[MaNccDM] [nvarchar](20) NULL,
	[SoLuong] [numeric](19, 3) NULL,
	[DGBan] [numeric](19, 4) NULL,
	[TriGiaBan] [numeric](19, 4) NULL,
	[TLCKGiamGia] [numeric](19, 3) NULL,
	[TienGiamGia] [numeric](19, 2) NULL,
	[TriGiaSauGiamGia] [numeric](19, 2) NULL,
	[TLCK_GioVang] [numeric](19, 3) NULL,
	[TienCK_GioVang] [numeric](19, 2) NULL,
	[TriGiaSauGioVang] [numeric](19, 2) NULL,
	[TLCKHD] [numeric](19, 3) NULL,
	[TienCKHD] [numeric](19, 2) NULL,
	[TriGiaSauCKHD] [numeric](19, 2) NULL,
	[TLCK_TheGiamGia] [numeric](19, 3) NULL,
	[TienCK_TheGiamGia] [numeric](19, 2) NULL,
	[ThanhTienBan] [numeric](19, 2) NULL,
	[TienPhuThu] [numeric](19, 2) NULL,
	[LoaiHangHoaBan] [int] NULL,
	[DaVanChuyen] [bit] NULL,
	[TenHH] [nvarchar](255) NULL,
	[RowID] [uniqueidentifier] NULL DEFAULT (newid()),
	
	PRIMARY KEY ([MaHD], [MaHH], [STT])
)