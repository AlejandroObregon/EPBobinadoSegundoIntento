// wwwroot/js/calendario-interactivo.js

class InteractiveCalendar {
    constructor(calendarId, options = {}) {
        this.calendarId = calendarId;
        this.currentDate = new Date();
        this.selectedDate = null;
        this.trainings = options.initialTrainings || {};
        this.options = {
            showTrainingPanel: options.showTrainingPanel !== false,
            onDateSelected: options.onDateSelected || null,
            onTrainingAdded: options.onTrainingAdded || null,
            onTrainingDeleted: options.onTrainingDeleted || null,
            ...options
        };

        this.monthNames = [
            'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
            'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
        ];

        this.init();
    }

    init() {
        this.render();
        this.bindEvents();
    }

    bindEvents() {
        const trainingInput = document.getElementById(`trainingInput-${this.calendarId}`);
        if (trainingInput) {
            trainingInput.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') this.addTraining();
            });
        }
    }

    previousMonth() {
        this.currentDate.setMonth(this.currentDate.getMonth() - 1);
        this.render();
    }

    nextMonth() {
        this.currentDate.setMonth(this.currentDate.getMonth() + 1);
        this.render();
    }

    render() {
        this.renderHeader();
        this.renderCalendar();
        if (this.options.showTrainingPanel) {
            this.renderTrainings();
        }
    }

    renderHeader() {
        const monthName = this.monthNames[this.currentDate.getMonth()];
        const year = this.currentDate.getFullYear();
        const monthElement = document.getElementById(`currentMonth-${this.calendarId}`);
        if (monthElement) {
            monthElement.textContent = `${monthName} ${year}`;
        }
    }

    renderCalendar() {
        const grid = document.getElementById(`calendarGrid-${this.calendarId}`);
        if (!grid) return;

        const dayCells = grid.querySelectorAll('.day-cell');
        dayCells.forEach(cell => cell.remove());

        const firstDay = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), 1);
        const lastDay = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() + 1, 0);
        const startDate = new Date(firstDay);
        startDate.setDate(startDate.getDate() - firstDay.getDay());

        const today = new Date();

        for (let i = 0; i < 42; i++) {
            const date = new Date(startDate);
            date.setDate(startDate.getDate() + i);

            const dayCell = document.createElement('div');
            dayCell.className = 'day-cell';
            dayCell.textContent = date.getDate();

            const dateKey = this.getDateKey(date);

            if (date.getMonth() !== this.currentDate.getMonth()) {
                dayCell.classList.add('other-month');
            }

            if (this.isToday(date, today)) {
                dayCell.classList.add('today');
            }

            if (this.selectedDate && this.getDateKey(date) === this.getDateKey(this.selectedDate)) {
                dayCell.classList.add('selected');
            }

            if (this.trainings[dateKey] && this.trainings[dateKey].length > 0) {
                dayCell.classList.add('has-training');
            }

            dayCell.addEventListener('click', () => this.selectDate(date));
            grid.appendChild(dayCell);
        }
    }

    selectDate(date) {
        this.selectedDate = new Date(date);
        this.render();

        if (this.options.onDateSelected) {
            this.options.onDateSelected(this.selectedDate);
        }
    }

    addTraining() {
        const typeSelect = document.getElementById(`trainingType-${this.calendarId}`);
        const input = document.getElementById(`trainingInput-${this.calendarId}`);
        const timeInput = document.getElementById(`trainingTime-${this.calendarId}`);
        const durationInput = document.getElementById(`trainingDuration-${this.calendarId}`);

        if (!typeSelect || !input || !timeInput || !durationInput) return;

        const type = typeSelect.value.trim();
        const description = input.value.trim();
        const time = timeInput.value;
        const duration = parseInt(durationInput.value) || 60;

        if (!type || !description || !this.selectedDate) {
            alert('Por favor selecciona una fecha, tipo de entrenamiento y descripción.');
            return;
        }

        const dateKey = this.getDateKey(this.selectedDate);
        if (!this.trainings[dateKey]) {
            this.trainings[dateKey] = [];
        }

        const newTraining = {
            id: Date.now(),
            type: type,
            description: description,
            time: time,
            duration: duration,
            date: new Date(this.selectedDate),
            dateKey: dateKey
        };

        this.trainings[dateKey].push(newTraining);

        // Limpiar formulario
        typeSelect.value = '';
        input.value = '';
        timeInput.value = '08:00';
        durationInput.value = '';

        this.render();

        if (this.options.onTrainingAdded) {
            this.options.onTrainingAdded(newTraining);
        }
    }

    deleteTraining(dateKey, trainingId) {
        if (this.trainings[dateKey]) {
            const trainingToDelete = this.trainings[dateKey].find(t => t.id === trainingId);
            this.trainings[dateKey] = this.trainings[dateKey].filter(training => training.id !== trainingId);

            if (this.trainings[dateKey].length === 0) {
                delete this.trainings[dateKey];
            }

            this.render();

            if (this.options.onTrainingDeleted && trainingToDelete) {
                this.options.onTrainingDeleted(trainingToDelete);
            }
        }
    }

    renderTrainings() {
        const trainingsList = document.getElementById(`trainingsList-${this.calendarId}`);
        if (!trainingsList) return;

        trainingsList.innerHTML = '';

        if (!this.selectedDate) {
            trainingsList.innerHTML = `
                <p style="color: #666; text-align: center; padding: 20px;">
                    🗓️ Selecciona una fecha para planificar entrenamientos
                </p>`;
            return;
        }

        const dateKey = this.getDateKey(this.selectedDate);
        const trainings = this.trainings[dateKey] || [];

        if (trainings.length === 0) {
            trainingsList.innerHTML = `
                <p style="color: #666; text-align: center; padding: 20px;">
                    💪 No hay entrenamientos programados para esta fecha
                </p>`;
            return;
        }

        // Ordenar entrenamientos por hora
        trainings.sort((a, b) => a.time.localeCompare(b.time));

        trainings.forEach(training => {
            const trainingItem = document.createElement('div');
            trainingItem.className = `training-item ${training.type.toLowerCase()}`;

            const icon = this.getTrainingIcon(training.type);

            trainingItem.innerHTML = `
                <div class="training-info">
                    <div class="training-icon">${icon}</div>
                    <div class="training-details">
                        <h4>${training.type}</h4>
                        <p>${this.escapeHtml(training.description)}</p>
                    </div>
                </div>
                <div class="training-meta">
                    <span>🕐 ${training.time}</span>
                    <span>⏱️ ${training.duration}min</span>
                    <button class="delete-training" onclick="getCalendarInstance('${this.calendarId}').deleteTraining('${dateKey}', ${training.id})">
                        🗑️ Eliminar
                    </button>
                </div>
            `;
            trainingsList.appendChild(trainingItem);
        });
    }

    getTrainingIcon(type) {
        const icons = {
            'Cardio': '🏃‍♂️',
            'Fuerza': '💪',
            'Flexibilidad': '🧘‍♀️',
            'HIIT': '⚡',
            'Natación': '🏊‍♀️',
            'Ciclismo': '🚴‍♂️',
            'Yoga': '🕉️',
            'CrossFit': '🔥',
            'Pilates': '🤸‍♀️',
            'Boxeo': '🥊'
        };
        return icons[type] || '🏋️‍♂️';
    }

    // Métodos públicos para manipular el calendario desde fuera
    setTrainings(trainings) {
        this.trainings = trainings || {};
        this.render();
    }

    getTrainings() {
        return this.trainings;
    }

    setDate(date) {
        this.currentDate = new Date(date);
        this.render();
    }

    selectDateProgrammatically(date) {
        this.selectDate(date);
    }

    // Métodos para obtener estadísticas de entrenamientos
    getTrainingStats() {
        const stats = {
            totalTrainings: 0,
            totalMinutes: 0,
            trainingsByType: {},
            trainingsByMonth: {}
        };

        Object.values(this.trainings).forEach(dayTrainings => {
            dayTrainings.forEach(training => {
                stats.totalTrainings++;
                stats.totalMinutes += training.duration;

                // Por tipo
                if (!stats.trainingsByType[training.type]) {
                    stats.trainingsByType[training.type] = { count: 0, minutes: 0 };
                }
                stats.trainingsByType[training.type].count++;
                stats.trainingsByType[training.type].minutes += training.duration;

                // Por mes
                const monthKey = `${training.date.getFullYear()}-${training.date.getMonth()}`;
                if (!stats.trainingsByMonth[monthKey]) {
                    stats.trainingsByMonth[monthKey] = { count: 0, minutes: 0 };
                }
                stats.trainingsByMonth[monthKey].count++;
                stats.trainingsByMonth[monthKey].minutes += training.duration;
            });
        });

        return stats;
    }

    // Métodos auxiliares
    getDateKey(date) {
        return `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
    }

    isToday(date, today) {
        return date.getDate() === today.getDate() &&
            date.getMonth() === today.getMonth() &&
            date.getFullYear() === today.getFullYear();
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
}

// Función global para acceder a las instancias de calendario
function getCalendarInstance(calendarId) {
    return window.calendarios ? window.calendarios[calendarId] : null;
}