import { Component, inject, OnInit } from "@angular/core";
import { Employee } from "../../models/employee";
import { EmployeeService } from "../../core/services/employee.service";
import { FormsModule } from "@angular/forms";

@Component({
    selector: 'app-resource-pool',
    imports: [FormsModule],
    templateUrl: './resource-pool.html',
    styleUrl: 'resource-pool.css'
})
export class Resourcepool implements OnInit {
    loading = false;
    error = '';
    minAvailability: number | null = null;
    private readonly employeeService = inject(EmployeeService)
    searchTerm = '';
    selectedSkill = '';
    selectedEmployee: Employee | null = null;

    Employees: Employee[] = [];

    ngOnInit(): void {
        this.loadEmployees();
    }

    viewProfile(employee: Employee): void {
        this.selectedEmployee = employee;
    }

    get filteredEmployees(): Employee[] {

        const search = this.searchTerm.trim().toLowerCase();

        return this.Employees.filter(employee => {

            const matchesSearch =
                !search ||
                employee.name.toLowerCase().includes(search) ||
                employee.designation.toLowerCase().includes(search) ||
                employee.location.toLowerCase().includes(search) ||
                employee.skills.some(skill =>
                    skill.toLowerCase().includes(search)
                );

            const availability =
                100 - employee.allocationPercentage;

            const matchesAvailability =
                this.minAvailability === null ||
                availability >= this.minAvailability;

            const matchesSkill =
                !this.selectedSkill ||
                employee.skills.some(skill =>
                    skill.toLowerCase() === this.selectedSkill.toLowerCase()
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
            this.Employees.flatMap(employee => employee.skills)
        )].sort();
    }

    loadEmployees(): void {
        this.loading = true;
        this.error = '';
        this.employeeService.getAllEmployees().subscribe({
            next: (employees) => {
                this.Employees = employees,
                    this.loading = false
            },

            error: (err) => {
                console.log(err);
                this.error = err;
                this.loading = false;
            }
        });
    }
}