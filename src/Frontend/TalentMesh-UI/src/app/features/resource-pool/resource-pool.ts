import { Component, inject, OnInit, signal } from "@angular/core";
import { Employee, EmployeeSkill } from "../../models/employee";
import { EmployeeService } from "../../core/services/employee.service";
import { FormsModule } from "@angular/forms";

@Component({
    selector: 'app-resource-pool',
    imports: [FormsModule],
    templateUrl: './resource-pool.html',
    styleUrl: 'resource-pool.css'
})
export class Resourcepool implements OnInit {
    loading = signal(false);
    error = signal('');
    minAvailability: number | null = null;
    private readonly employeeService = inject(EmployeeService)
    searchTerm = signal('');
    selectedSkill = signal('');
    selectedEmployee: Employee | null = null;

    Employees: Employee[] = [];

    ngOnInit(): void {
        this.loadEmployees();
    }

    viewProfile(employee: Employee): void {
        this.selectedEmployee = employee;
    }

    get filteredEmployees(): Employee[] {

        const search = this.searchTerm().trim().toLowerCase();
        const selectedSkill = this.selectedSkill().trim().toLowerCase();

        return this.Employees.filter(employee => {

            const matchesSearch =
                !search ||
                employee.name.toLowerCase().includes(search) ||
                employee.designation.toLowerCase().includes(search) ||
                employee.location.toLowerCase().includes(search) ||
                employee.skills.some(skill =>
                    skill.skill.name.toLowerCase().includes(search)
                );

            const availability =
                100 - employee.allocationPercentage;

            const matchesAvailability =
                this.minAvailability === null ||
                availability >= this.minAvailability;

            const matchesSkill =
                !selectedSkill ||
                employee.skills.some(employeeSkill =>
                    employeeSkill.skill.name.toLowerCase() === selectedSkill
                );

            return (
                matchesSearch &&
                matchesAvailability &&
                matchesSkill
            );
        });
    }

    get availableSkills(): string[] {

        return [...new Set(
            this.Employees.flatMap(employee => employee.skills.map(x => x.skill.name))
        )].sort();
    }

    loadEmployees(): void {
        this.loading.set(true);
        this.error.set('');

        this.employeeService.getAllEmployees().subscribe({
            next: (employees) => {
                this.Employees = employees,
                    this.loading.set(false);
                console.log(this.Employees);
            },

            error: (err) => {
                console.log(err);
                this.error = err;
                this.loading.set(false);
            }
        });
    }
}