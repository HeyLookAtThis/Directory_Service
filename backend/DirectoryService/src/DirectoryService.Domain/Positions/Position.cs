using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Positions
{
    public class Position
    {
        private readonly List<DepartmentPosition> _departments = [];

        private Position(Guid id, Name name)
        {
            Id = id;
            Name = name;
            IsActive = true;
            CreateAt = DateTime.UtcNow;
            UpdateAt = CreateAt;
        }

        public Guid Id { get; private set; }

        public Name Name { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreateAt { get; private set; }

        public DateTime UpdateAt { get; private set; }

        public IReadOnlyList<DepartmentPosition> Departments => _departments;

        public static Result<Position, Error> Create(string name, string? description)
        {
            string invalidField = "Position";

            Result<Name, Error> nameResult = Name.Create(name, invalidField);

            if (nameResult.IsFailure)
                return nameResult.Error;

            return new Position(Guid.NewGuid(), nameResult.Value);
        }

        public void Delete()
        {
            IsActive = false;
            ResetUpdatingTime();
        }

        public void AddDepartment(Guid departmentId)
        {
            _departments.Add(new DepartmentPosition(departmentId, Id));
            ResetUpdatingTime();
        }

        public Result<bool, Error> TryToRemovePosition(Guid departmentId)
        {
            var removingDepartmentPosition =
                _departments.FirstOrDefault(departmentPosition => departmentPosition.DepartmentId == departmentId);

            if (removingDepartmentPosition != null)
            {
                _departments.Remove(removingDepartmentPosition);
                ResetUpdatingTime();
                return true;
            }

            return Error.NotFound("position " + Id, "отсутствует подразделение " + departmentId, "position");
        }

        private void ResetUpdatingTime() => UpdateAt = DateTime.UtcNow;
    }
}