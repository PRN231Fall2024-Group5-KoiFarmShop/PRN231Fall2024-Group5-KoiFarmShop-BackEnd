﻿using Koi.DTOs.WalletDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IWalletService
    {
        Task<DepositResponseDTO> Deposit(long amount);
    }
}