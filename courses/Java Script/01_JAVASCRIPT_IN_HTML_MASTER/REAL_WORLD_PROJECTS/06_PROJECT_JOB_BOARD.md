# 💼 Project 21: Job Board Platform

## Job Listings and Applications

---

## Table of Contents

1. [Job Listings](#job-listings)
2. [Application System](#application-system)
3. [Search and Filters](#search-and-filters)
4. [Dashboard](#dashboard)

---

## Job Listings

### Job Structure

```javascript
const jobSchema = {
  id: 'job-001',
  company: {
    name: 'Tech Corp',
    logo: '/logos/techcorp.jpg',
    website: 'https://techcorp.com'
  },
  title: 'Senior JavaScript Developer',
  location: 'Remote',
  type: 'Full-time',
  salary: { min: 100000, max: 150000, currency: 'USD' },
  description: 'We are looking for an experienced developer...',
  requirements: ['5+ years experience', 'React', 'Node.js'],
  postedAt: new Date(),
  tags: ['javascript', 'react', 'remote']
};
```

### Job Card

```javascript
class JobCard {
  constructor(job) {
    this.job = job;
  }
  
  render() {
    return `
      <div class="job-card">
        <img src="${this.job.company.logo}" alt="${this.job.company.name}">
        <h3>${this.job.title}</h3>
        <p class="company">${this.job.company.name}</p>
        <p class="location">${this.job.location}</p>
        <p class="salary">$${this.job.salary.min} - $${this.job.salary.max}</p>
        <div class="tags">
          ${this.job.tags.map(t => `<span>${t}</span>`).join('')}
        </div>
        <button data-action="apply" data-job="${this.job.id}">Apply</button>
      </div>
    `;
  }
}
```

---

## Application System

### Application Form

```javascript
class ApplicationForm {
  constructor(jobId) {
    this.jobId = jobId;
    this.files = {};
  }
  
  async submit(formData) {
    const application = {
      jobId: this.jobId,
      name: formData.name,
      email: formData.email,
      phone: formData.phone,
      resume: formData.resume,
      coverLetter: formData.coverLetter,
      appliedAt: new Date()
    };
    
    return await API.apply(application);
  }
  
  validate() {
    return true;
  }
}
```

---

## Search and Filters

### Search Engine

```javascript
class JobSearch {
  constructor() {
    this.filters = {
      keyword: '',
      location: '',
      type: '',
      salaryMin: 0,
      tags: []
    };
  }
  
  async search(filters) {
    this.filters = { ...this.filters, ...filters };
    
    let jobs = await API.getJobs();
    
    if (filters.keyword) {
      jobs = jobs.filter(j => 
        j.title.toLowerCase().includes(filters.keyword.toLowerCase()) ||
        j.description.toLowerCase().includes(filters.keyword.toLowerCase())
      );
    }
    
    if (filters.location) {
      jobs = jobs.filter(j => j.location === filters.location);
    }
    
    if (filters.type) {
      jobs = jobs.filter(j => j.type === filters.type);
    }
    
    return jobs;
  }
}
```

---

## Dashboard

### Application Tracker

```javascript
class ApplicationDashboard {
  constructor(userId) {
    this.userId = userId;
    this.applications = [];
  }
  
  async load() {
    this.applications = await API.getApplications(this.userId);
    this.render();
  }
  
  getStatusCounts() {
    return {
      total: this.applications.length,
      pending: this.applications.filter(a => a.status === 'pending').length,
      interview: this.applications.filter(a => a.status === 'interview').length,
      rejected: this.applications.filter(a => a.status === 'rejected').length,
      accepted: this.applications.filter(a => a.status === 'accepted').length
    };
  }
}
```

---

## Summary

### Key Takeaways

1. **Job Listings**: Structured data
2. **Search**: Filter system
3. **Applications**: Form and tracking

### Next Steps

- Continue with: [07_PROJECT_WEATHER_STATION.md](07_PROJECT_WEATHER_STATION.md)
- Add saved jobs
- Implement job alerts

---

## Cross-References

- **Previous**: [05_PROJECT_SOCIAL_NETWORK.md](05_PROJECT_SOCIAL_NETWORK.md)
- **Next**: [07_PROJECT_WEATHER_STATION.md](07_PROJECT_WEATHER_STATION.md)

---

*Last updated: 2024*