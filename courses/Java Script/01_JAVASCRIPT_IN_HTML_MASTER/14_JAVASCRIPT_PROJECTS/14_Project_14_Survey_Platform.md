# 📝 Project 14: Survey Platform

## 📋 Project Overview

Build a survey creation and response collection platform with multiple question types, results analytics, and export functionality. This project demonstrates:
- Dynamic form building
- Multiple question types
- Response collection
- Results visualization

---

## 🎯 Core Features

```javascript
class SurveyPlatform {
    constructor() {
        this.surveys = [];
        this.loadFromStorage();
    }
    
    createSurvey(title, questions) {
        const survey = {
            id: this.generateId(),
            title,
            questions: questions.map((q, index) => ({
                id: this.generateId(),
                type: q.type, // text, radio, checkbox, rating
                question: q.question,
                options: q.options || [],
                required: q.required || false
            })),
            responses: [],
            createdAt: new Date().toISOString()
        };
        
        this.surveys.push(survey);
        this.saveToStorage();
        return survey;
    }
    
    submitResponse(surveyId, answers) {
        const survey = this.surveys.find(s => s.id === surveyId);
        if (!survey) throw new Error('Survey not found');
        
        survey.responses.push({
            id: this.generateId(),
            answers,
            submittedAt: new Date().toISOString()
        });
        
        this.saveToStorage();
    }
    
    getResults(surveyId) {
        const survey = this.surveys.find(s => s.id === surveyId);
        if (!survey) return null;
        
        const results = {};
        survey.questions.forEach(q => {
            results[q.id] = {
                question: q.question,
                type: q.type,
                responses: survey.responses.map(r => r.answers[q.id])
            };
        });
        
        return results;
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('surveys', JSON.stringify(this.surveys));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('surveys');
        if (stored) {
            try {
                this.surveys = JSON.parse(stored);
            } catch (e) {
                this.surveys = [];
            }
        }
    }
}
```

---

## 🔗 Related Topics

- [09_Conditional_Statements.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/09_Conditional_Statements.md)
- [04_Creating_and_Adding_Elements.md](../09_DOM_MANIPULATION/04_Creating_and_Adding_Elements.md)

---

**Next: [Document Editor](./15_Project_15_Document_Editor.md)**