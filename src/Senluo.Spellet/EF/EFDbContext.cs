using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.EF
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base() { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Answer>().ToTable("Answer");
            modelBuilder.Entity<Answer>().HasKey(o => o.ID);
            modelBuilder.Entity<AnswerSheet>().ToTable("AnswerSheet");
            modelBuilder.Entity<AnswerSheet>().HasKey(o => o.ID);
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Course>().HasKey(o => o.ID);
            modelBuilder.Entity<CourseContent>().ToTable("CourseContent");
            modelBuilder.Entity<CourseContent>().HasKey(o => o.ID);
            modelBuilder.Entity<CourseContent>().Ignore(o => o.Entry);
            modelBuilder.Entity<Entry>().ToTable("Entry");
            modelBuilder.Entity<Entry>().HasKey(o => o.ID);
            modelBuilder.Entity<Exam>().ToTable("Exam");
            modelBuilder.Entity<Exam>().HasKey(o => o.ID);
            modelBuilder.Entity<Example>().ToTable("Example");
            modelBuilder.Entity<Example>().HasKey(o => o.ID);
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<Question>().HasKey(o => o.ID);
            modelBuilder.Entity<Question>().Ignore(o => o.Example);
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Student>().HasKey(o => o.ID);
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Teacher>().HasKey(o => o.ID);
            modelBuilder.Entity<Translation>().ToTable("Translation");
            modelBuilder.Entity<Translation>().HasKey(o => o.ID);
            
        }
    }
}