using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Locations
{
    public class Location
    {
        private readonly List<DepartmentLocation> _departments = [];

        private Location(Guid id, Name name, Address address)
        {
            Id = id;
            Name = name;
            Address = address;
            IsActive = true;
            CreateAt = DateTime.UtcNow;
            UpdateAt = CreateAt;
        }

        public Guid Id { get; }

        public Name Name { get; private set; }

        public Address Address { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreateAt { get; private set; }

        public DateTime UpdateAt { get; private set; }

        public IReadOnlyList<DepartmentLocation> Departments => _departments;

        public static Result<Location, Error> Create(
            string name,
            short postalCode,
            string country,
            string region,
            string city,
            string street,
            string house)
        {
            string invalidField = "location";

            Result<Name, Error> nameResult = Name.Create(name, invalidField);

            if (nameResult.IsFailure)
                return nameResult.Error;

            Result<Address, Error> addressResult = Address.Create(postalCode, country, region, city, street, house, invalidField);

            if (addressResult.IsFailure)
                return addressResult.Error;

            return new Location(
                Guid.NewGuid(),
                nameResult.Value,
                addressResult.Value);
        }

        public void Delete()
        {
            IsActive = false;
            ResetUpdatingTime();
        }

        public void AddDepartment(Guid departmentId)
        {
            _departments.Add(new DepartmentLocation(departmentId, Id));
            ResetUpdatingTime();
        }

        public Result<bool, Error> TryToRemoveDepartment(Guid departmentId)
        {
            var removingDepartmentLocation =
                _departments.FirstOrDefault(departmentLocation => departmentLocation.DepartmentId == departmentId);

            if (removingDepartmentLocation != null)
            {
                _departments.Remove(removingDepartmentLocation);
                ResetUpdatingTime();
                return true;
            }

            return Error.NotFound("location " + Id, "отсутствует подразделение " + departmentId, "location");
        }

        private void ResetUpdatingTime() => UpdateAt = DateTime.UtcNow;
    }
}