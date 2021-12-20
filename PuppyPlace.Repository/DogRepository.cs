﻿using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;
using PuppyPlace.Domain;

namespace PuppyPlace.Repository;

public class DogRepository
{
    private PuppyPlaceContext _context;
    
    public DogRepository(PuppyPlaceContext context)
    {
        _context = context;
    }

    public async Task AddDogAsync(Dog dog)
    {
        await _context.Dogs.AddAsync(dog);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Dog>> DogsAsync()
    {
        return await _context.Dogs.ToListAsync();
    }

    public async Task<Dog?> FindByIdAsync(Guid id)
    {
        return await _context.Dogs.Include(d => d.Owners).FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task RemoveDogAsync(Dog dog)
    {
        _context.Dogs.Remove(dog);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNameAsync(Dog dog)
    {
        _context.Dogs.Update(dog);
        await _context.SaveChangesAsync();
    }
}