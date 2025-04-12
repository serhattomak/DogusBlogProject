
document.addEventListener('DOMContentLoaded', () => {
    // Initialize components
    initCreateBlogComponent();
    renderBlogFeed();
    renderSidebars();

    // Initialize lucide icons
    if (typeof lucide !== 'undefined') {
        lucide.createIcons();
    }
});

// Create blog component functionality
function initCreateBlogComponent() {
    const createBlogTrigger = document.getElementById('create-blog-trigger');
    const createBlogExpanded = document.getElementById('create-blog-expanded');
    const cancelCreateBlog = document.getElementById('cancel-create-blog');
    const tagInput = document.getElementById('tag-input');
    const tagsContainer = document.getElementById('tags-container');

    if (!createBlogTrigger || !createBlogExpanded || !isAuthenticated) {
        return;
    }

    createBlogTrigger.addEventListener('click', () => {
        createBlogTrigger.classList.add('d-none');
        createBlogExpanded.classList.remove('d-none');
    });

    if (cancelCreateBlog) {
        cancelCreateBlog.addEventListener('click', () => {
            createBlogExpanded.classList.add('d-none');
            createBlogTrigger.classList.remove('d-none');
        });
    }

    if (tagInput) {
        tagInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' || e.key === ',') {
                e.preventDefault();
                const tagValue = tagInput.value.trim();
                if (tagValue) {
                    addTag(tagValue);
                    tagInput.value = '';
                }
            }
        });
    }

    function addTag(tagText) {
        const tag = document.createElement('div');
        tag.className = 'tag-badge d-flex align-items-center';
        tag.innerHTML = `
      #${tagText}
      <button type="button" class="btn btn-sm p-0 ms-1">
        <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M18 6 6 18"></path><path d="m6 6 12 12"></path></svg>
      </button>
    `;

        const removeButton = tag.querySelector('button');
        removeButton.addEventListener('click', () => {
            tag.remove();
        });

        tagsContainer.insertBefore(tag, tagInput);
    }
}

// Render the blog feed
function renderBlogFeed() {
    const blogFeedElement = document.getElementById('blog-feed');

    if (!blogFeedElement) {
        return;
    }

    mockBlogPosts.forEach(post => {
        const blogPost = createBlogPostElement(post);
        blogFeedElement.appendChild(blogPost);
    });
}

// Create a blog post element
function createBlogPostElement(post) {
    const postElement = document.createElement('div');
    postElement.className = 'card mb-3';
    postElement.dataset.postId = post.id;

    const formattedDate = formatDate(post.createdAt);

    postElement.innerHTML = `
    <div class="card-body">
      <div class="d-flex">
        <div class="me-3">
          <img 
            src="${post.author.avatar || "https://via.placeholder.com/40"}" 
            alt="${post.author.name}"
            class="rounded-circle"
            width="48"
            height="48"
          />
        </div>
        <div class="flex-grow-1">
          <div class="d-flex align-items-center mb-1">
            <a href="profile.html" class="fw-bold blog-post-author me-2 text-decoration-none">
              ${post.author.name}
            </a>
            <span class="blog-post-timestamp">
              · ${formattedDate}
            </span>
          </div>
          
          <a href="#" class="text-decoration-none text-dark">
            <h2 class="fw-bold fs-5 mb-1">${post.title}</h2>
            <p class="blog-post-content">${post.content}</p>
          </a>
          
          <div class="blog-post-tags mb-3">
            <a href="category-detail.html?id=${post.categoryId}" class="text-secondary me-2 text-decoration-none">
              ${post.category}
            </a>
            ${post.tags.map(tag => `
              <a href="#" class="blog-post-tag me-2">
                #${tag}
              </a>
            `).join('')}
          </div>
          
          <div class="blog-post-actions d-flex justify-content-between">
            <button 
              class="btn blog-post-action-btn comment-btn" 
              data-post-id="${post.id}"
              data-post-title="${post.title}"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1"><path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"></path></svg>
              ${post.commentsCount}
            </button>
            
            <button
              class="btn blog-post-action-btn like-btn ${post.isLiked ? "liked" : ""}"
              data-post-id="${post.id}"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="${post.isLiked ? 'currentColor' : 'none'}" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="me-1"><path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z"></path></svg>
              <span class="likes-count">${post.likesCount}</span>
            </button>
            
            <button
              class="btn blog-post-action-btn bookmark-btn ${post.isBookmarked ? "bookmarked" : ""}"
              data-post-id="${post.id}"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="${post.isBookmarked ? 'currentColor' : 'none'}" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m19 21-7-4-7 4V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v16z"></path></svg>
            </button>
          </div>
        </div>
      </div>
    </div>
  `;

    // Add event listeners for post actions
    initPostActions(postElement, post);

    return postElement;
}

// Initialize event listeners for post actions
function initPostActions(postElement, post) {
    const commentBtn = postElement.querySelector('.comment-btn');
    const likeBtn = postElement.querySelector('.like-btn');
    const bookmarkBtn = postElement.querySelector('.bookmark-btn');

    // Comment button functionality
    if (commentBtn) {
        commentBtn.addEventListener('click', () => {
            if (isAuthenticated) {
                const commentModal = new bootstrap.Modal(document.getElementById('commentModal'));
                document.getElementById('commentModalLabel').textContent = `Comment on "${post.title}"`;
                commentModal.show();
            } else {
                const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                loginModal.show();
            }
        });
    }

    // Like button functionality
    if (likeBtn) {
        likeBtn.addEventListener('click', () => {
            if (!isAuthenticated) {
                const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                loginModal.show();
                return;
            }

            const likeIcon = likeBtn.querySelector('svg');
            const likesCountElement = likeBtn.querySelector('.likes-count');
            const currentLikes = parseInt(likesCountElement.textContent);

            if (likeBtn.classList.toggle('liked')) {
                likeIcon.setAttribute('fill', 'currentColor');
                likesCountElement.textContent = currentLikes + 1;
            } else {
                likeIcon.setAttribute('fill', 'none');
                likesCountElement.textContent = currentLikes - 1;
            }
        });
    }

    // Bookmark button functionality
    if (bookmarkBtn) {
        bookmarkBtn.addEventListener('click', () => {
            if (!isAuthenticated) {
                const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                loginModal.show();
                return;
            }

            const bookmarkIcon = bookmarkBtn.querySelector('svg');

            if (bookmarkBtn.classList.toggle('bookmarked')) {
                bookmarkIcon.setAttribute('fill', 'currentColor');
            } else {
                bookmarkIcon.setAttribute('fill', 'none');
            }
        });
    }
}

// Render categories and tags in sidebars
function renderSidebars() {
    renderCategoriesList();
    renderTagsList();
}

// Render categories list in sidebar
function renderCategoriesList() {
    const categoriesListElement = document.getElementById('categories-list');

    if (!categoriesListElement) {
        return;
    }

    mockCategories.forEach(category => {
        const categoryElement = document.createElement('a');
        categoryElement.href = `category-detail.html?id=${category.id}`;
        categoryElement.className = 'list-group-item list-group-item-action d-flex justify-content-between align-items-center';
        categoryElement.innerHTML = `
      <span>${category.name}</span>
      <span class="badge bg-primary rounded-pill">${category.count}</span>
    `;

        categoriesListElement.appendChild(categoryElement);
    });
}

// Render tags list in sidebar
function renderTagsList() {
    const tagsListElement = document.getElementById('tags-list');

    if (!tagsListElement) {
        return;
    }

    mockTags.forEach(tag => {
        const tagElement = document.createElement('a');
        tagElement.href = `tag-detail.html?id=${tag.id}`;
        tagElement.className = 'tag-badge';
        tagElement.innerHTML = `
      #${tag.name} <span class="text-secondary">(${tag.count})</span>
    `;

        tagsListElement.appendChild(tagElement);
    });
}

// Helper function to format dates
function formatDate(date) {
    try {
        return dateFns.formatDistanceToNow(new Date(date), { addSuffix: true });
    } catch (e) {
        // Fallback if date-fns is not available
        return new Date(date).toLocaleString();
    }
}
