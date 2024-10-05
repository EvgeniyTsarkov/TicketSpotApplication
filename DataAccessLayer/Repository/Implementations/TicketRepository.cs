﻿using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class TicketRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Ticket>(ticketSpotContext), ITicketRepository
{
    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        var itemToUpdate = await Get(x => x.Id == ticket.Id)
            ?? throw new RecordNotFoundException("The ticket to be updated is not found in the database");

        _context.ChangeTracker.Clear();

        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Ticket with id: {0} is not found in the database", id));

        _context.ChangeTracker.Clear();

        _context.Tickets.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
