﻿using EMS.Groups;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace EMS.GroupMembers;

public class GroupMemberManager : DomainService
{
    private readonly IGroupMemberRepository _groupMemberRepository;

    public GroupMemberManager(IGroupMemberRepository groupMemberRepository)
    {
        _groupMemberRepository = groupMemberRepository;
    }
    public async Task<GroupMember> CreateAsync([NotNull] Guid userId, [NotNull] Guid groupId, [NotNull] bool isRemoved, [NotNull] DateTime dateOfJoin)
    {
        var existingGroup = await _groupMemberRepository.FirstOrDefaultAsync(m => m.userId == userId && m.groupId == groupId);
        if (existingGroup != null)
        {
            throw new MemberAlreadyExistsException(userId);
        }
        return new GroupMember(
            GuidGenerator.Create(),
                userId: userId,
                groupId: groupId,
                isRemoved: isRemoved,
                dateOfJoin: dateOfJoin
            );
    }
    public async Task<List<GroupMember>> CreateListAsync(List<GroupMember> input)
    {
        var output = new List<GroupMember>();
        foreach (var groupMember in input)
        {
            var newGroupMember = await CreateAsync(
                userId: groupMember.userId,
                groupId: groupMember.groupId,
                isRemoved: groupMember.isRemoved,
                dateOfJoin: groupMember.dateOfJoin
            );
            output.Add(newGroupMember);
        }
        return output;
    }
}
