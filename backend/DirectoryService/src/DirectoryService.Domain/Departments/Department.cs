using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain.Departments
{
    public class Department
    {
        private readonly List<DepartmentLocation> _locations = [];
        private readonly List<DepartmentPosition> _positions = [];

        private Department(Guid id, Name name, Slug slug, Guid? parentId, Path path)
        {
            Id = id;
            Name = name;
            Slug = slug;
            ParentId = parentId;
            Path = path;
            IsActive = true;
            CreateAt = DateTime.UtcNow;
            UpdateAt = CreateAt;
        }

        public Guid Id { get; private set; }

        public Name Name { get; private set; }

        public Slug Slug { get; private set; }

        public Path Path { get; private set; }

        public Guid? ParentId { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreateAt { get; private set; }

        public DateTime UpdateAt { get; private set; }

        public IReadOnlyList<DepartmentLocation> Locations => _locations;

        public IReadOnlyList<DepartmentPosition> Positions => _positions;

        public static Result<Department, Error> Create(string name, string slug, Guid? parentId, Path? parentPath)
        {
            string invalidField = "department";

            Result<Name, Error> nameResult = Name.Create(name, invalidField);

            if (nameResult.IsFailure)
                return nameResult.Error;

            Result<Slug, Error> slugResult = Slug.Create(slug, invalidField);

            if (slugResult.IsFailure)
                return slugResult.Error;

            Path path = new(parentPath, slug);

            return new Department(Guid.NewGuid(), nameResult.Value, slugResult.Value, parentId, path);
        }

        public void AddLocation(Guid locationId)
        {
            _locations.Add(new DepartmentLocation(Id, locationId));
            ResetUpdatingTime();
        }

        public void AddPosition(Guid positionId)
        {
            _positions.Add(new DepartmentPosition(Id, positionId));
            ResetUpdatingTime();
        }

        public Result<bool, Error> TryToRemoveLocation(Guid locationId)
        {
            var removingDepartmentLocation =
                _locations.FirstOrDefault(departmentLocation => departmentLocation.LocationId == locationId);

            if (removingDepartmentLocation != null)
            {
                _locations.Remove(removingDepartmentLocation);
                ResetUpdatingTime();
                return true;
            }

            return Error.NotFound("department " + Id, "отсутствует локация " + locationId, "department");
        }

        public Result<bool, Error> TryToRemovePosition(Guid positionId)
        {
            var removingDepartmentPosition =
                _positions.FirstOrDefault(departmentPosition => departmentPosition.PositionId == positionId);

            if (removingDepartmentPosition != null)
            {
                _positions.Remove(removingDepartmentPosition);
                ResetUpdatingTime();
                return true;
            }

            return Error.NotFound("department " + Id, "отсутствует позиция " + positionId, "department");
        }

        public void Delete()
        {
            IsActive = false;
            ResetUpdatingTime();
        }

        private void ResetUpdatingTime() => UpdateAt = DateTime.UtcNow;
    }
}