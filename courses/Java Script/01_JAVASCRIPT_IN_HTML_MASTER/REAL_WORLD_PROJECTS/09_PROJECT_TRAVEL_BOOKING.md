# ✈️ Project 24: Travel Booking Platform

## Flight and Hotel Booking System

---

## Table of Contents

1. [System Overview](#system-overview)
2. [Flight Search](#flight-search)
3. [Hotel Search](#hotel-search)
4. [Booking Management](#booking-management)
5. [Payment Integration](#payment-integration)
6. [User Dashboard](#user-dashboard)

---

## System Overview

### Architecture

```
┌─────────────────────────────────────────────────────────────┐
│              TRAVEL BOOKING PLATFORM                         │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐   │
│  │   Flight   │ │  Hotel   │ │   Train   │   │
│  │   Search   │ │   Search  │ │   Search  │   │
│  └──────────────┘ └──────────────┘ └──────────────┘   │
│                      │                                    │
│                      ▼                                    │
│            ┌─────────────────┐                            │
│            │    Booking     │                            │
│            │    Engine     │                            │
│            └─────────────────┘                            │
│                      │                                    │
│                      ▼                                    │
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐   │
│  │   Payment  │ │   User    │ │  Itinerary  │   │
│  │   Gateway  │ │  Dashboard │ │   Manager  │   │
│  └──────────────┘ └──────────────┘ └──────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

### Data Models

```javascript
const flightSchema = {
  id: 'FL-001',
  airline: 'Delta Airlines',
  flightNumber: 'DL1234',
  origin: {
    code: 'JFK',
    city: 'New York',
    time: '2024-06-15T10:00:00Z'
  },
  destination: {
    code: 'LAX',
    city: 'Los Angeles',
    time: '2024-06-15T13:30:00Z'
  },
  price: 299.99,
  class: 'economy',
  available: 45,
  duration: '5h 30m',
  stops: 0
};

const hotelSchema = {
  id: 'HTL-001',
  name: 'Grand Hotel',
  location: {
    city: 'Los Angeles',
    address: '123 Ocean Drive',
    coordinates: { lat: 34.0522, lng: -118.2437 }
  },
  rating: 4.5,
  reviewCount: 1250,
  pricePerNight: 199.99,
  amenities: ['wifi', 'pool', 'gym', 'restaurant'],
  images: ['/hotel1.jpg', '/hotel2.jpg'],
  rooms: [
    { type: 'standard', available: 5, price: 199.99 },
    { type: 'deluxe', available: 3, price: 299.99 },
    { type: 'suite', available: 1, price: 499.99 }
  ]
};
```

---

## Flight Search

### Search Form Component

```javascript
class FlightSearchForm {
  constructor(container) {
    this.container = container;
    this.formData = {
      origin: '',
      destination: '',
      departureDate: '',
      returnDate: '',
      passengers: 1,
      class: 'economy'
    };
  }

  render() {
    this.container.innerHTML = `
      <form class="flight-search-form">
        <div class="search-fields">
          <div class="field">
            <label>From</label>
            <input type="text" name="origin" placeholder="City or airport" required>
          </div>
          <div class="field">
            <label>To</label>
            <input type="text" name="destination" placeholder="City or airport" required>
          </div>
          <div class="field">
            <label>Departure</label>
            <input type="date" name="departure" required>
          </div>
          <div class="field">
            <label>Return</label>
            <input type="date" name="return">
          </div>
          <div class="field">
            <label>Passengers</label>
            <select name="passengers">
              <option value="1">1 Adult</option>
              <option value="2">2 Adults</option>
              <option value="3">3 Adults</option>
            </select>
          </div>
          <div class="field">
            <label>Class</label>
            <select name="class">
              <option value="economy">Economy</option>
              <option value="business">Business</option>
              <option value="first">First Class</option>
            </select>
          </div>
        </div>
        <button type="submit" class="search-btn">Search Flights</button>
      </form>
    `;
  }

  async search() {
    const results = await FlightAPI.search(this.formData);
    return results;
  }
}
```

### Flight Results Display

```javascript
class FlightResults {
  constructor(flights) {
    this.flights = flights;
  }

  render() {
    const html = this.flights.map(flight => `
      <div class="flight-card">
        <div class="airline-info">
          <img src="${flight.airlineLogo}" alt="${flight.airline}">
          <span>${flight.airline}</span>
          <span class="flight-number">${flight.flightNumber}</span>
        </div>
        <div class="flight-times">
          <div class="departure">
            <span class="time">${flight.origin.time}</span>
            <span class="code">${flight.origin.code}</span>
          </div>
          <div class="duration">
            <span>${flight.duration}</span>
            <span class="${flight.stops > 0 ? 'stops' : 'direct'}">
              ${flight.stops === 0 ? 'Direct' : `${flight.stops} stop`}
            </span>
          </div>
          <div class="arrival">
            <span class="time">${flight.destination.time}</span>
            <span class="code">${flight.destination.code}</span>
          </div>
        </div>
        <div class="flight-price">
          <span class="price">$${flight.price}</span>
          <span class="seats">${flight.available} seats left</span>
          <button class="select-btn" data-flight="${flight.id}">Select</button>
        </div>
      </div>
    `).join('');

    return `<div class="flight-results">${html}</div>`;
  }

  sortBy(sortBy) {
    switch(sortBy) {
      case 'price-low':
        return this.flights.sort((a, b) => a.price - b.price);
      case 'price-high':
        return this.flights.sort((a, b) => b.price - a.price);
      case 'duration':
        return this.flights.sort((a, b) => a.durationMinutes - b.durationMinutes);
    }
  }

  filterBy(filters) {
    return this.flights.filter(flight => {
      if (filters.maxPrice && flight.price > filters.maxPrice) return false;
      if (filters.stops !== undefined && flight.stops !== filters.stops) return false;
      if (filters.departureTime && flight.departureTime < filters.departureTime) return false;
      return true;
    });
  }
}
```

---

## Hotel Search

### Hotel Search Implementation

```javascript
class HotelSearch {
  constructor() {
    this.results = [];
  }

  async search(criteria) {
    const params = new URLSearchParams(criteria);
    const response = await fetch(`/api/hotels/search?${params}`);
    this.results = await response.json();
    return this.results;
  }

  renderResults(mapFunction) {
    return this.results.map(hotel => mapFunction(hotel)).join('');
  }

  getMapMarkers() {
    return this.results.map(hotel => ({
      position: hotel.location.coordinates,
      title: hotel.name,
      info: `$${hotel.pricePerNight}/night`
    }));
  }
}

class HotelCard {
  constructor(hotel) {
    this.hotel = hotel;
  }

  render() {
    const stars = '★'.repeat(Math.floor(this.hotel.rating));
    return `
      <div class="hotel-card">
        <img src="${this.hotel.images[0]}" alt="${this.hotel.name}">
        <div class="hotel-info">
          <h3>${this.hotel.name}</h3>
          <p class="location">${this.hotel.location.city}</p>
          <div class="rating">${stars} ${this.hotel.rating} (${this.hotel.reviewCount})</div>
          <div class="amenities">
            ${this.hotel.amenities.slice(0, 4).map(a => `<span>${a}</span>`).join('')}
          </div>
        </div>
        <div class="hotel-price">
          <span class="price">$${this.hotel.pricePerNight}</span>
          <span class="per-night">per night</span>
          <button class="book-btn" data-hotel="${this.hotel.id}">Book</button>
        </div>
      </div>
    `;
  }
}
```

---

## Booking Management

### Booking Engine

```javascript
class BookingEngine {
  constructor() {
    this.cart = {
      flights: [],
      hotels: [],
      passengers: [],
      totalPrice: 0
    };
  }

  addFlight(flight, passengers) {
    this.cart.flights.push({ flight, passengers });
    this.calculateTotal();
  }

  addHotel(hotel, checkIn, checkOut, rooms) {
    const nights = Math.ceil((checkOut - checkIn) / (1000 * 60 * 60 * 24));
    this.cart.hotels.push({ hotel, checkIn, checkOut, rooms, nights });
    this.calculateTotal();
  }

  calculateTotal() {
    let total = 0;
    
    this.cart.flights.forEach(booking => {
      total += booking.flight.price * booking.passengers;
    });
    
    this.cart.hotels.forEach(booking => {
      total += booking.hotel.pricePerNight * booking.rooms * booking.nights;
    });
    
    this.cart.totalPrice = total;
    return total;
  }

  removeItem(itemId, type) {
    if (type === 'flight') {
      this.cart.flights = this.cart.flights.filter(f => f.flight.id !== itemId);
    } else if (type === 'hotel') {
      this.cart.hotels = this.cart.hotels.filter(h => h.hotel.id !== itemId);
    }
    this.calculateTotal();
  }

  async confirmBooking(paymentDetails) {
    const booking = {
      id: `BK-${Date.now()}`,
      items: this.cart,
      payment: paymentDetails,
      status: 'confirmed',
      createdAt: new Date()
    };

    const confirmation = await BookingAPI.create(booking);
    this.cart = { flights: [], hotels: [], passengers: [], totalPrice: 0 };
    
    return confirmation;
  }

  getItinerary() {
    return {
      flights: this.cart.flights.map(f => ({
        ...f.flight,
        passengers: f.passengers
      })),
      hotels: this.cart.hotels.map(h => ({
        name: h.hotel.name,
        checkIn: h.checkIn,
        checkOut: h.checkOut,
        rooms: h.rooms
      })),
      totalPrice: this.cart.totalPrice
    };
  }
}
```

---

## Payment Integration

### Payment Gateway

```javascript
class PaymentGateway {
  constructor() {
    this.providers = {
      stripe: new StripeProvider(),
      paypal: new PayPalProvider()
    };
  }

  async processPayment(paymentDetails) {
    const { method, amount, currency, bookingId } = paymentDetails;
    
    try {
      const provider = this.providers[method];
      const result = await provider.charge({
        amount: amount * 100,
        currency: currency || 'USD',
        description: `Booking ${bookingId}`
      });
      
      return {
        success: true,
        transactionId: result.id,
        status: result.status
      };
    } catch (error) {
      return {
        success: false,
        error: error.message
      };
    }
  }

  async refund(bookingId, amount) {
    const refund = await PaymentAPI.refund({ bookingId, amount });
    return refund;
  }
}

class StripeProvider {
  async charge(details) {
    const response = await fetch('/api/payments/stripe', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(details)
    });
    return response.json();
  }
}

class PayPalProvider {
  async charge(details) {
    const response = await fetch('/api/payments/paypal', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(details)
    });
    return response.json();
  }
}
```

---

## User Dashboard

### My Bookings Dashboard

```javascript
class BookingDashboard {
  constructor(userId) {
    this.userId = userId;
    this.bookings = [];
  }

  async loadBookings() {
    this.bookings = await BookingAPI.getUserBookings(this.userId);
  }

  getUpcoming() {
    const now = new Date();
    return this.bookings.filter(b => new Date(b.departureDate) > now);
  }

  getPast() {
    const now = new Date();
    return this.bookings.filter(b => new Date(b.departureDate) < now);
  }

  render() {
    const upcoming = this.getUpcoming();
    const past = this.getPast();

    return `
      <div class="booking-dashboard">
        <section class="upcoming-bookings">
          <h2>Upcoming Trips</h2>
          ${upcoming.map(booking => this.renderBookingCard(booking)).join('')}
        </section>
        <section class="past-bookings">
          <h2>Past Trips</h2>
          ${past.map(booking => this.renderBookingCard(booking)).join('')}
        </section>
      </div>
    `;
  }

  renderBookingCard(booking) {
    return `
      <div class="booking-card">
        <div class="booking-header">
          <span class="booking-id">${booking.id}</span>
          <span class="status ${booking.status}">${booking.status}</span>
        </div>
        <div class="booking-details">
          <div class="flight-info">
            <strong>${booking.flights[0].origin.code}</strong>
            <span>→</span>
            <strong>${booking.flights[0].destination.code}</strong>
          </div>
          <div class="dates">
            ${new Date(booking.departureDate).toLocaleDateString()}
          </div>
        </div>
        <div class="booking-actions">
          <button class="view-btn">View Details</button>
          <button class="cancel-btn" data-id="${booking.id}">Cancel</button>
        </div>
      </div>
    `;
  }
}
```

---

## Summary

### Key Takeaways

1. **Flight Search**: Multi-filter search with sorting
2. **Hotel Search**: Map integration, ratings
3. **Booking Engine**: Cart management
4. **Payment**: Multiple providers
5. **Dashboard**: User management

### Next Steps

- Continue with: [10_PROJECT_NEWS_PLATFORM.md](10_PROJECT_NEWS_PLATFORM.md)

---

## Cross-References

- **Previous**: [08_PROJECT_MARKETPLACE.md](08_PROJECT_MARKETPLACE.md)
- **Next**: [10_PROJECT_NEWS_PLATFORM.md](10_PROJECT_NEWS_PLATFORM.md)

---

*Last updated: 2024*