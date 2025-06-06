﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserRoleAsync(int userId, string newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
    }
}
