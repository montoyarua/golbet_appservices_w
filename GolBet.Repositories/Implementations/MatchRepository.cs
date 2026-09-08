// GolBet.Repositories/Implementations/MatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class MatchRepository : GenericRepository<Match>, IMatchRepository
{
    public MatchRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null)
    {
        var query = _dbSet
            .Include(m => m.HomeTeam) // Include its similar to JOIN in SQL, it allows you to load related entities along with the main entity.
            .Include(m => m.AwayTeam)
            .Where(m => m.IsActive)
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(m => m.Status == status.Value);

        return await query.OrderBy(m => m.Date).ToListAsync(); // Its necesary use .ToListAsync ever when you use IEnumerable because the query is not executed until you enumerate it. The ToListAsync method forces the execution of the query and returns the results as a list.
    }

    public async Task<Match?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Bets)
            .AsNoTracking() // AsNoTracking is used to improve query performance, since we do not need to track changes to the retrieved entities
            .FirstOrDefaultAsync(m => m.Id == id); /* FirstOrDefaultAsync returns the first element of a sequence, or a default value if no element is found. In this case, it will return null if no match with the specified id is found
                                                    * Is necesary when you use Team? or other nullable types, because if you use FirstAsync and no match is found, it will throw an exception. */

}