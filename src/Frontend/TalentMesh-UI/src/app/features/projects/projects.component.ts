import { Component, inject, OnInit, signal } from "@angular/core";
import { ProjectService } from "../../core/services/project.service";
import { Project } from "../../models/project";
import { Router } from "@angular/router";

@Component({
    selector: 'app-projects',
    templateUrl: './projects.component.html',
    styleUrl: './projects.component.css',
})
export class ProjectComponent implements OnInit {

    private projectService = inject(ProjectService);
    private router = inject(Router);

    projects = signal<Project[]>([]);

    loading = signal(false);
    error = signal('');

    ngOnInit(): void {
        console.log('Projects component initialized');
        this.loadProjects();
    }

    loadProjects(): void {
        console.log('Loading projects...');
        this.loading.set(true);
        this.projectService.getProjects().subscribe({
            next: (projects) => {
                console.log('Projects received:', projects);
                this.projects.set(projects);
                this.loading.set(false);
            },
            error: (err) => {
                console.log(err);
                this.error.set(err);
                this.loading.set(false);
            }
        });
    }

    createProject(): void {
        this.router.navigate(['/projects/create']);
    }
}