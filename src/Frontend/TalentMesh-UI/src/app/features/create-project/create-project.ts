import { Component, inject } from '@angular/core';
import {
    FormArray,
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { ProjectService } from '../../core/services/project.service';

type SkillForm = FormGroup<{
    skillName: ReturnType<FormBuilder['control']>;
    requiredLevel: ReturnType<FormBuilder['control']>;
    requiredCount: ReturnType<FormBuilder['control']>;
}>;

interface SkillRequirementForm {
    skillName: string;
    requiredLevel: string;
    requiredCount: number;
}

@Component({
    selector: 'app-create-project',
    imports: [ReactiveFormsModule],
    templateUrl: './create-project.html',
    styleUrl: './create-project.css'
})
export class CreateProject {

    private fb = inject(FormBuilder);
    private projectService = inject(ProjectService);
    private router = inject(Router);

    submitting = false;
    error = '';

    projectForm = this.fb.group({

        name: [
            '',
            [
                Validators.required,
                Validators.maxLength(200)
            ]
        ],

        description: [
            '',
            Validators.required
        ],

        clientName: [
            '',
            Validators.required
        ],

        startDate: [
            '',
            Validators.required
        ],

        endDate: [
            '',
            Validators.required
        ],

        skillRequirements: this.fb.array([])

    });

    get skillRequirements(): FormArray<FormGroup> {
        return this.projectForm.get(
            'skillRequirements'
        ) as FormArray<FormGroup>;
    }

    addSkill(): void {

        const skill = this.fb.group({

            skillName: [
                '',
                Validators.required
            ],

            requiredLevel: [
                'Intermediate',
                Validators.required
            ],

            requiredCount: [
                1,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ]

        });

        this.skillRequirements.push(skill);
    }

    removeSkill(index: number): void {
        this.skillRequirements.removeAt(index);
    }

    submit(): void {

        if (this.projectForm.invalid) {

            this.projectForm.markAllAsTouched();

            return;
        }

        this.submitting = true;
        this.error = '';

        const formValue =
            this.projectForm.getRawValue();
        const skills =
            formValue.skillRequirements as SkillRequirementForm[];

        const request = {
            name: formValue.name!,
            description: formValue.description!,
            clientName: formValue.clientName!,
            startDate: `${formValue.startDate}T00:00:00Z`,
            endDate: `${formValue.endDate}T00:00:00Z`,

            skillRequirements:
                skills.map(skill => ({
                    skillName: skill.skillName!,
                    requiredLevel: skill.requiredLevel!,
                    requiredCount: skill.requiredCount!
                }))
        };

        this.projectService
            .createProject(request)
            .subscribe({

                next: () => {

                    this.submitting = false;

                    this.router.navigate(['/projects']);

                },

                error: (err) => {

                    console.error(err);

                    this.error =
                        'Unable to create project.';

                    this.submitting = false;
                }

            });
    }

    cancel(): void {
        this.router.navigate(['/projects']);
    }
}